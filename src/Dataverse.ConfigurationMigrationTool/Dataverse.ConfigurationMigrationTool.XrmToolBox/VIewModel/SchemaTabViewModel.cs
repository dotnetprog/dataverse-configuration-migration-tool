using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Common;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.VIewModel
{
    public class SchemaTabViewModel
    {
        //MultiSelect is type of virtual
        private static IMapper<EntityMetadata, EntitySchema> entitySchemaMapper = new EntityMetadataToEntitySchemaMapper();
        private static IMapper<AttributeMetadata, FieldSchema> attributeSchemaMapper = new AttributeMetadataToFIeldSchemaMapper();
        private static IMapper<ManyToManyRelationshipMetadata, RelationshipSchema> relationshipSchemaMapper = new ManyToManyRelationshipMetadataToRelationshipSchema();
        private static readonly AttributeTypeCode[] ExcludedAttributeTypes = new[]
        {
            AttributeTypeCode.Virtual,
            AttributeTypeCode.PartyList,
            AttributeTypeCode.ManagedProperty,
            AttributeTypeCode.CalendarRules,
            AttributeTypeCode.EntityName
        };
        public Dictionary<string, List<SchemaItem>> SelectedSchemaItems = new Dictionary<string, List<SchemaItem>>();
        public EntityMetadata CurrentSelectedEntity { get; set; }

        public BindingList<FieldComponentMetadataViewModel> entityMetadataViewModels = new BindingList<FieldComponentMetadataViewModel>()
        {
            AllowRemove = true,
            AllowNew = false
        };
        public BindingList<RelationshipComponentMetadataViewModel> relationshipViewModels = new BindingList<RelationshipComponentMetadataViewModel>()
        {
            AllowRemove = true,
            AllowNew = false
        };
        public void ClearMetadata()
        {
            this.entityMetadataViewModels.Clear();
            this.relationshipViewModels.Clear();
        }
        public void SetEntityMetadataSelection(EntityMetadata entityMetadata)
        {
            CurrentSelectedEntity = entityMetadata;
            var fields = entityMetadata
           .Attributes
           .Where(a => a.IsValidForCreate == true &&
                       a.IsValidForUpdate == true &&
                       a.AttributeType != null &&
                       !ExcludedAttributeTypes.Contains(a.AttributeType.Value))
           .OrderBy(a => a.DisplayName?.UserLocalizedLabel?.Label ?? a.LogicalName)
           .Select(a => new FieldComponentMetadataViewModel
           {
               LogicalName = a.LogicalName,
               DisplayName = a.DisplayName?.UserLocalizedLabel?.Label ?? a.LogicalName,
               DataType = a.AttributeType.ToString(),
               IsSelected = false
           })
           .ToList();
            this.entityMetadataViewModels.AddRange(fields);

            var relationships = entityMetadata.ManyToManyRelationships
            .OrderBy(r => r.SchemaName)
            .Where(r => r.Entity1LogicalName == entityMetadata.LogicalName)
            .Select(r => new RelationshipComponentMetadataViewModel
            {
                TableName = r.IntersectEntityName,
                SchemaName = r.SchemaName,
                TargetEntity = r.Entity2LogicalName,
                IsSelected = false
            }).ToList();
            this.relationshipViewModels.AddRange(relationships);
        }
        public void MoveRight()
        {
            // Move to next tab logic


            List<SchemaItem> schemaItems;
            if (!SelectedSchemaItems.TryGetValue(CurrentSelectedEntity.LogicalName, out schemaItems))
            {
                schemaItems = new List<SchemaItem>();
                SelectedSchemaItems[CurrentSelectedEntity.LogicalName] = schemaItems;
            }
            var fields = this.entityMetadataViewModels
                .Where(f => f.IsSelected && schemaItems.Where(i => i.schemaItemType == SchemaItemType.Field).All(i => i.Name != f.LogicalName));

            var relationships = this.relationshipViewModels.Where(r => r.IsSelected &&
            schemaItems.Where(i => i.schemaItemType == SchemaItemType.Relationship).All(i => i.Name != r.SchemaName));
            schemaItems.AddRange(fields.Select(f => new FieldSchemaItem
            {
                Name = f.LogicalName,
                IsSelected = false,
                OriginalItem = f
            }).ToList());
            schemaItems.AddRange(relationships.Select(r => new RelationshipSchemaItem
            {
                Name = r.SchemaName,
                IsSelected = false,
                OriginalItem = r
            }).ToList());



        }
        public void MoveLeft(IEnumerable<KeyValuePair<string, SchemaItem[]>> items)
        {
            foreach (var kv in items)
            {
                if (!SelectedSchemaItems.ContainsKey(kv.Key)) continue;
                var list = SelectedSchemaItems[kv.Key];
                foreach (var item in kv.Value)
                {
                    list.Remove(item);
                }
            }
        }
        public void Reset()
        {

            this.ClearMetadata();
            this.SelectedSchemaItems.Clear();
            this.CurrentSelectedEntity = null;
        }
        public DataSchema GenerateSchema(IEnumerable<EntityMetadata> AllEntitiesMetadata)
        {
            var schema = new DataSchema()
            {
                Entity = new List<EntitySchema>()
            };
            foreach (var kv in SelectedSchemaItems.OrderBy(k => k.Key))
            {
                var entityMetadata = AllEntitiesMetadata.FirstOrDefault(e => e.LogicalName == kv.Key);
                if (entityMetadata == null) continue;

                var EntitySchema = entitySchemaMapper.Map(entityMetadata);
                schema.Entity.Add(EntitySchema);
                foreach (var fieldSchemaItem in kv.Value.Where(i => i.schemaItemType == SchemaItemType.Field))
                {
                    var fieldMetadata = entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == fieldSchemaItem.Name);
                    if (fieldMetadata == null) continue;
                    var fieldSchema = attributeSchemaMapper.Map(fieldMetadata);
                    EntitySchema.Fields.Field.Add(fieldSchema);
                }
                //Implement Relationship schema
                foreach (var relationshipSchemaItem in kv.Value.Where(i => i.schemaItemType == SchemaItemType.Relationship))
                {
                    var relationshipMetadata = entityMetadata.ManyToManyRelationships.FirstOrDefault(r => r.SchemaName == relationshipSchemaItem.Name);
                    if (relationshipMetadata == null) continue;
                    var relationshipSchema = relationshipSchemaMapper.Map(relationshipMetadata);
                    EntitySchema.Relationships.Relationship.Add(relationshipSchema);
                }
            }
            return schema;
        }
    }
}
