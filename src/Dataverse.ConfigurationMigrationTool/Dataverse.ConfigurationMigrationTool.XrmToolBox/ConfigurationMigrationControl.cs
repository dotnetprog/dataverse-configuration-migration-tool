using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain.Abstraction;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.Services;
using Dataverse.ConfigurationMigrationTool.XrmToolBox.VIewModel;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox
{
    public partial class ConfigurationMigrationControl : PluginControlBase, IAboutPlugin, IHelpPlugin, IGitHubPlugin
    {
        private Settings mySettings;
        private string ToolVersion { get; }
        private static readonly IFileService fileService = new XmlFileService();
        private ISolutionService solutionService { get; set; }
        private IMetadataService metadataService { get; set; }
        private SchemaTabViewModel schemaTabViewModel = new SchemaTabViewModel();
        private IReadOnlyCollection<EntityMetadata> entityMetadataCache { get; set; }

        public string HelpUrl => "https://github.com/dotnetprog/dataverse-configuration-migration-tool";

        public string RepositoryName => "dataverse-configuration-migration-tool";

        public string UserName => "dotnetprog";

        public ConfigurationMigrationControl()
        {
            InitializeComponent();
            InitializeInformativeText();
            InitializeEntityComponentsDataGridView();
            InitializeRelationshipComponentsDataGridView();
            var fvi = FileVersionInfo.GetVersionInfo(this.GetType().Assembly.Location);
            this.ToolVersion = fvi.FileVersion;
        }
        private void InitializeInformativeText()
        {
            var info = @"{\rtf1\ansi\deff0
    {\fonttbl{\f0\fnil\fcharset0 Calibri;}}
                \par {\b **Information**}
                \par Currently , only schema file generation is available with this tool. \par

                \par Data Export and Data Import are planned to be available soon. 
                \par In the mean time, the generated schema file can be used to import and export data using the cli tool in the project repo.
                \par A service principal that have permissions to your dataverse environment will be needed.\par

                \par {\b\i Project repo:}
                \par https://github.com/dotnetprog/dataverse-configuration-migration-tool
                ";
            this.informativeRTC.Text = null;
            this.informativeRTC.Rtf = info;

        }
        #region Grid Initialization
        private void InitializeEntityComponentsDataGridView()
        {
            this.entityMetadataGrid.AutoGenerateColumns = false;
            this.entityMetadataGrid.DataSource = schemaTabViewModel.entityMetadataViewModels;
            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Select",
                Name = nameof(FieldComponentMetadataViewModel.IsSelected),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                DataPropertyName = nameof(FieldComponentMetadataViewModel.IsSelected),

            };
            this.entityMetadataGrid.Columns.Insert(0, checkboxColumn);
            this.entityMetadataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Display Name",
                DataPropertyName = nameof(FieldComponentMetadataViewModel.DisplayName),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                MinimumWidth = 200
            });
            this.entityMetadataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "LogicalName",
                DataPropertyName = nameof(FieldComponentMetadataViewModel.LogicalName),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 200,
                ReadOnly = true
            });
            this.entityMetadataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "DataType",
                DataPropertyName = nameof(FieldComponentMetadataViewModel.DataType),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                ReadOnly = true
            });

        }
        private void InitializeRelationshipComponentsDataGridView()
        {
            this.relationshipm2m_grid.AutoGenerateColumns = false;
            this.relationshipm2m_grid.DataSource = schemaTabViewModel.relationshipViewModels;
            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Select",
                Name = nameof(RelationshipComponentMetadataViewModel.IsSelected),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                DataPropertyName = nameof(RelationshipComponentMetadataViewModel.IsSelected)
            };
            this.relationshipm2m_grid.Columns.Insert(0, checkboxColumn);
            this.relationshipm2m_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Schema Name",
                DataPropertyName = nameof(RelationshipComponentMetadataViewModel.SchemaName),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                ReadOnly = true,
                MinimumWidth = 200
            });
            this.relationshipm2m_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Table Intersecct Name",
                DataPropertyName = nameof(RelationshipComponentMetadataViewModel.TableName),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 200,
                ReadOnly = true
            });
            this.relationshipm2m_grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Target Entity",
                DataPropertyName = nameof(RelationshipComponentMetadataViewModel.TargetEntity),
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                MinimumWidth = 300
            });

        }
        #endregion

        #region Grid Events
        private void relationshipm2m_grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure it's the checkbox column and a valid row
            if (e.ColumnIndex == relationshipm2m_grid.Columns[nameof(RelationshipComponentMetadataViewModel.IsSelected)].Index && e.RowIndex >= 0)
            {
                // Toggle the checkbox state
                var item = this.schemaTabViewModel.relationshipViewModels[e.RowIndex];
                item.IsSelected = !item.IsSelected;
                // Optionally, update row selection based on checkbox state

                relationshipm2m_grid.Rows[e.RowIndex].Selected = item.IsSelected;
                if (!item.IsSelected)
                {
                    fromentity_select_all_cbox.Checked = false;
                }


            }
        }
        private void entityMetadataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure it's the checkbox column and a valid row
            if (e.ColumnIndex == entityMetadataGrid.Columns[nameof(FieldComponentMetadataViewModel.IsSelected)].Index && e.RowIndex >= 0)
            {
                // Toggle the checkbox state
                var item = this.schemaTabViewModel.entityMetadataViewModels[e.RowIndex];
                item.IsSelected = !item.IsSelected;
                // Optionally, update row selection based on checkbox state

                entityMetadataGrid.Rows[e.RowIndex].Selected = item.IsSelected;
                if (!item.IsSelected)
                {
                    fromentity_select_all_cbox.Checked = false;
                }


            }
        }
        #endregion

        #region Tool Events
        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }


        private void SolutionLoadBtn_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadSolutions);
        }
        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }
        private void ResetControl()
        {
            this.schemaitem_listview.Items.Clear();
            this.schemaitem_listview.Groups.Clear();
            this.entitylist_cbox.DataSource = null;
            this.solutionsCombbox.DataSource = null;


        }
        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            ResetControl();
            this.schemaTabViewModel.Reset();
            base.UpdateConnection(newService, detail, actionName, parameter);
            InitializeServices();
            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }
        private void InitializeServices()
        {
            this.solutionService = new DataverseSolutionService(Service);
            this.metadataService = new DataverseMetadataService(Service);
        }
        #endregion

        #region Async Workers

        private void GenerateAndExportSchemaFile()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Generating Schema File",
                Work = (worker, args) =>
                {

                    args.Result = this.schemaTabViewModel.GenerateSchema(this.entityMetadataCache);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as DataSchema;
                    ExportSchemaFile(result);
                }
            });
        }
        private void ExportSchemaFile(DataSchema schema)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML|*.xml";
            dialog.Title = "Export Schema Definition File";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                fileService.WriteToFile(dialog.FileName, schema);
            }
        }
        private void LoadEntityMetadata(string entityName)
        {

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entity Metadata",
                Work = (worker, args) =>
                {

                    args.Result = metadataService.GetEntityMetadata(entityName);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityMetadata;
                    this.schemaTabViewModel.SetEntityMetadataSelection(result);
                }
            });
        }

        private void LoadEntitiesBySolutionId(Guid solutionId)
        {
            entitylist_cbox.SelectedIndex = -1; // Reset selection
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities",
                Work = (worker, args) =>
                {
                    args.Result = solutionService.GetEntitiesBySolutionId(solutionId);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as IReadOnlyCollection<SolutionEntityComponent>;
                    var filteredReseult = (from r in result
                                           join emd in this.entityMetadataCache on r.EntityName equals emd.LogicalName
                                           where emd.IsIntersect != true
                                           select r).ToList();
                    entitylist_cbox.DataSource = filteredReseult;
                    entitylist_cbox.Enabled = filteredReseult.Any();
                }
            });
        }
        private void LoadSolutions()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Solutions",
                Work = (worker, args) =>
                {
                    args.Result = solutionService.GetSolutions();

                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    this.solutionsCombbox.Text = string.Empty;
                    var result = args.Result as IReadOnlyCollection<Solution>;
                    this.solutionsCombbox.DataSource = result;



                }
            });
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities",
                Work = (worker, args) =>
                {
                    args.Result = metadataService.GetAllEntityMetadata();

                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    this.entityMetadataCache = (args.Result as IEnumerable<EntityMetadata>).ToArray();
                }
            });
        }
        #endregion


        #region TabSchema UI Handlers
        private void solutioncbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteMethod(() =>
            {
                this.fromentity_select_all_cbox.Checked = false;
                this.schemaTabViewModel.ClearMetadata();
                var selectedSolution = this.solutionsCombbox.SelectedItem as Solution;
                if (selectedSolution != null)
                {
                    LoadEntitiesBySolutionId(selectedSolution.Id);
                }
            });
        }
        private void entitylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteMethod(() =>
            {
                this.fromentity_select_all_cbox.Checked = false;
                this.schemaTabViewModel.ClearMetadata();
                var selectedEntity = this.entitylist_cbox.SelectedItem as SolutionEntityComponent;
                if (selectedEntity != null)
                {
                    LoadEntityMetadata(selectedEntity.EntityName);
                }
            });
        }


        private void entityMetadataGrid_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in entityMetadataGrid.Rows)
            {
                var item = row.DataBoundItem as FieldComponentMetadataViewModel;
                row.Selected = item.IsSelected;
            }
        }
        private void relationshipm2m_grid_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in relationshipm2m_grid.Rows)
            {
                var item = row.DataBoundItem as RelationshipComponentMetadataViewModel;
                row.Selected = item.IsSelected;
            }
        }

        private void fromentity_select_all_cbox_Click(object sender, EventArgs e)
        {

            Task.Run(() =>
            {
                foreach (DataGridViewRow row in entityMetadataGrid.Rows)
                {
                    if (row.DataBoundItem is FieldComponentMetadataViewModel item)
                    {
                        item.IsSelected = fromentity_select_all_cbox.Checked;
                        row.Selected = item.IsSelected; // Update the selection state of the row
                    }
                }
            });
            Task.Run(() =>
            {
                foreach (DataGridViewRow row in relationshipm2m_grid.Rows)
                {
                    if (row.DataBoundItem is RelationshipComponentMetadataViewModel item)
                    {
                        item.IsSelected = fromentity_select_all_cbox.Checked;
                        row.Selected = item.IsSelected; // Update the selection state of the row
                    }
                }
            });
        }



        private void moveRightSchema_btn_Click(object sender, EventArgs e)
        {
            this.schemaTabViewModel.MoveRight();

            this.schemaitem_listview.Items.Clear();
            foreach (var item in this.schemaTabViewModel.SelectedSchemaItems)
            {
                var listViewGroup = new ListViewGroup(item.Key, HorizontalAlignment.Left);
                this.schemaitem_listview.Groups.Add(listViewGroup);
                var items = item.Value
                    .OrderBy(i => i.schemaItemType)
                    .ThenBy(i => i.Name)
                    .Select(i => CreateListViewItem(i, listViewGroup))
                    .ToList();
                this.schemaitem_listview.Items.AddRange(items.ToArray());
            }

        }
        private ListViewItem CreateListViewItem(SchemaItem item, ListViewGroup group)
        {
            var listViewItem = new ListViewItem(item.Name, group);
            listViewItem.SubItems.Add(item.schemaItemType.ToString());
            listViewItem.Tag = item; // Store the SchemaItem in the Tag property for later use
            return listViewItem;
        }

        private void moveLeftSchema_btn_Click(object sender, EventArgs e)
        {
            var selectedItems = this.schemaitem_listview.CheckedItems.Cast<ListViewItem>().ToList();
            var items = selectedItems.Select(i => (i.Group.Header, i.Tag as SchemaItem))
                .GroupBy(i => i.Header)
                .Select(i => new KeyValuePair<string, SchemaItem[]>(i.Key, i.Select(x => x.Item2).ToArray()))
                .ToList();
            this.schemaTabViewModel.MoveLeft(items);
            foreach (var item in selectedItems)
            {
                this.schemaitem_listview.Items.Remove(item);
            }
            foreach (var group in this.schemaitem_listview.Groups.Cast<ListViewGroup>().Where(i => i.Items.Count == 0).ToList())
            {

                this.schemaitem_listview.Groups.Remove(group);

            }

        }


        private void GenerateSchemaFileBtn_Click(object sender, EventArgs e)
        {
            GenerateAndExportSchemaFile();
        }

        public void ShowAboutDialog()
        {
            MessageBox.Show($@"Configuration Migration Tool For XrmToolBox is bascially the out of the box Configuration Migration Tool provided by Microsoft.{Environment.NewLine}Currently, only the schema creation functionality is implemented in this tool.{Environment.NewLine}You can use the custom cli tool to import/export data with the schema file generated by this tool located in the github repo of this tool.{Environment.NewLine}{Environment.NewLine}Version: {ToolVersion}"
        , $"ConfigurationMigrationTool.XrmToolBox v{ToolVersion}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}