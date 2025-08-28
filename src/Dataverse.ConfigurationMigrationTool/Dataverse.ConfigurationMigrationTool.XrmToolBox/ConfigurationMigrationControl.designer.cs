using Dataverse.ConfigurationMigrationTool.XrmToolBox.Properties;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox
{
    partial class ConfigurationMigrationControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationMigrationControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SolutionLoadBtn = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Schema = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.solutionsCombbox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.entitylist_cbox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.fromselection_lbl = new System.Windows.Forms.Label();
            this.fromentity_select_all_cbox = new System.Windows.Forms.CheckBox();
            this.entityMetadataGrid = new System.Windows.Forms.DataGridView();
            this.relationshipm2m_grid = new System.Windows.Forms.DataGridView();
            this.moveRightSchema_btn = new System.Windows.Forms.Button();
            this.moveLeftSchema_btn = new System.Windows.Forms.Button();
            this.schemaitem_listview = new System.Windows.Forms.ListView();
            this.ColumnDisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnDisplayType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.schemaDefinition_lbl = new System.Windows.Forms.Label();
            this.GenerateSchemaFileBtn = new System.Windows.Forms.Button();
            this.informativeRTC = new System.Windows.Forms.RichTextBox();
            this.toolStripMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Schema.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityMetadataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.relationshipm2m_grid)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.SolutionLoadBtn});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(990, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SolutionLoadBtn
            // 
            this.SolutionLoadBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SolutionLoadBtn.Image = ((System.Drawing.Image)(resources.GetObject("SolutionLoadBtn.Image")));
            this.SolutionLoadBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolutionLoadBtn.Name = "SolutionLoadBtn";
            this.SolutionLoadBtn.Size = new System.Drawing.Size(89, 22);
            this.SolutionLoadBtn.Text = "Load Solutions";
            this.SolutionLoadBtn.Click += new System.EventHandler(this.SolutionLoadBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Schema);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(990, 810);
            this.tabControl1.TabIndex = 5;
            // 
            // Schema
            // 
            this.Schema.Controls.Add(this.tableLayoutPanel1);
            this.Schema.Location = new System.Drawing.Point(4, 22);
            this.Schema.Name = "Schema";
            this.Schema.Padding = new System.Windows.Forms.Padding(3);
            this.Schema.Size = new System.Drawing.Size(982, 784);
            this.Schema.TabIndex = 0;
            this.Schema.Text = "Schema";
            this.Schema.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.entityMetadataGrid, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.relationshipm2m_grid, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.moveRightSchema_btn, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.moveLeftSchema_btn, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.schemaitem_listview, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.informativeRTC, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(976, 778);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(432, 124);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.solutionsCombbox);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(270, 53);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a solution";
            // 
            // solutionsCombbox
            // 
            this.solutionsCombbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.solutionsCombbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.solutionsCombbox.FormattingEnabled = true;
            this.solutionsCombbox.Location = new System.Drawing.Point(3, 16);
            this.solutionsCombbox.MinimumSize = new System.Drawing.Size(200, 0);
            this.solutionsCombbox.Name = "solutionsCombbox";
            this.solutionsCombbox.Size = new System.Drawing.Size(267, 21);
            this.solutionsCombbox.TabIndex = 1;
            this.solutionsCombbox.Text = "Please load solutions first";
            this.solutionsCombbox.SelectedIndexChanged += new System.EventHandler(this.solutioncbox_SelectedIndexChanged);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.entitylist_cbox);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 62);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(270, 53);
            this.flowLayoutPanel3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(267, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Choose entity";
            // 
            // entitylist_cbox
            // 
            this.entitylist_cbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.entitylist_cbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.entitylist_cbox.Enabled = false;
            this.entitylist_cbox.FormattingEnabled = true;
            this.entitylist_cbox.Location = new System.Drawing.Point(3, 16);
            this.entitylist_cbox.MinimumSize = new System.Drawing.Size(200, 0);
            this.entitylist_cbox.Name = "entitylist_cbox";
            this.entitylist_cbox.Size = new System.Drawing.Size(267, 21);
            this.entitylist_cbox.TabIndex = 1;
            this.entitylist_cbox.SelectedIndexChanged += new System.EventHandler(this.entitylist_SelectedIndexChanged);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.flowLayoutPanel5);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 133);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(432, 53);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel5.Controls.Add(this.fromselection_lbl);
            this.flowLayoutPanel5.Controls.Add(this.fromentity_select_all_cbox);
            this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(488, 48);
            this.flowLayoutPanel5.TabIndex = 0;
            // 
            // fromselection_lbl
            // 
            this.fromselection_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fromselection_lbl.AutoSize = true;
            this.fromselection_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromselection_lbl.Location = new System.Drawing.Point(3, 0);
            this.fromselection_lbl.Name = "fromselection_lbl";
            this.fromselection_lbl.Size = new System.Drawing.Size(274, 13);
            this.fromselection_lbl.TabIndex = 0;
            this.fromselection_lbl.Text = "Select Fields and relationships for your schema";
            this.fromselection_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fromentity_select_all_cbox
            // 
            this.fromentity_select_all_cbox.AutoSize = true;
            this.fromentity_select_all_cbox.Location = new System.Drawing.Point(3, 23);
            this.fromentity_select_all_cbox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.fromentity_select_all_cbox.Name = "fromentity_select_all_cbox";
            this.fromentity_select_all_cbox.Size = new System.Drawing.Size(70, 17);
            this.fromentity_select_all_cbox.TabIndex = 1;
            this.fromentity_select_all_cbox.Text = "Select All";
            this.fromentity_select_all_cbox.UseVisualStyleBackColor = true;
            this.fromentity_select_all_cbox.Click += new System.EventHandler(this.fromentity_select_all_cbox_Click);
            // 
            // entityMetadataGrid
            // 
            this.entityMetadataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.entityMetadataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityMetadataGrid.Location = new System.Drawing.Point(3, 192);
            this.entityMetadataGrid.Name = "entityMetadataGrid";
            this.entityMetadataGrid.Size = new System.Drawing.Size(432, 288);
            this.entityMetadataGrid.TabIndex = 1;
            this.entityMetadataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.entityMetadataGrid_CellClick);
            this.entityMetadataGrid.SelectionChanged += new System.EventHandler(this.entityMetadataGrid_SelectionChanged);
            // 
            // relationshipm2m_grid
            // 
            this.relationshipm2m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.relationshipm2m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.relationshipm2m_grid.Location = new System.Drawing.Point(3, 486);
            this.relationshipm2m_grid.Name = "relationshipm2m_grid";
            this.relationshipm2m_grid.Size = new System.Drawing.Size(432, 289);
            this.relationshipm2m_grid.TabIndex = 3;
            this.relationshipm2m_grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.relationshipm2m_grid_CellClick);
            this.relationshipm2m_grid.SelectionChanged += new System.EventHandler(this.relationshipm2m_grid_SelectionChanged);
            // 
            // moveRightSchema_btn
            // 
            this.moveRightSchema_btn.BackgroundImage = global::Dataverse.ConfigurationMigrationTool.XrmToolBox.Properties.Resources.right_arrow;
            this.moveRightSchema_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveRightSchema_btn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.moveRightSchema_btn.Location = new System.Drawing.Point(441, 403);
            this.moveRightSchema_btn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.moveRightSchema_btn.Name = "moveRightSchema_btn";
            this.moveRightSchema_btn.Size = new System.Drawing.Size(94, 50);
            this.moveRightSchema_btn.TabIndex = 4;
            this.moveRightSchema_btn.UseVisualStyleBackColor = true;
            this.moveRightSchema_btn.Click += new System.EventHandler(this.moveRightSchema_btn_Click);
            // 
            // moveLeftSchema_btn
            // 
            this.moveLeftSchema_btn.BackgroundImage = global::Dataverse.ConfigurationMigrationTool.XrmToolBox.Properties.Resources.left_arrow;
            this.moveLeftSchema_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveLeftSchema_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.moveLeftSchema_btn.Location = new System.Drawing.Point(441, 513);
            this.moveLeftSchema_btn.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.moveLeftSchema_btn.Name = "moveLeftSchema_btn";
            this.moveLeftSchema_btn.Size = new System.Drawing.Size(94, 50);
            this.moveLeftSchema_btn.TabIndex = 5;
            this.moveLeftSchema_btn.UseVisualStyleBackColor = true;
            this.moveLeftSchema_btn.Click += new System.EventHandler(this.moveLeftSchema_btn_Click);
            // 
            // schemaitem_listview
            // 
            this.schemaitem_listview.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.schemaitem_listview.CheckBoxes = true;
            this.schemaitem_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnDisplayName,
            this.ColumnDisplayType});
            this.schemaitem_listview.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.schemaitem_listview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaitem_listview.FullRowSelect = true;
            this.schemaitem_listview.GridLines = true;
            this.schemaitem_listview.HideSelection = false;
            this.schemaitem_listview.Location = new System.Drawing.Point(541, 192);
            this.schemaitem_listview.Name = "schemaitem_listview";
            this.tableLayoutPanel1.SetRowSpan(this.schemaitem_listview, 2);
            this.schemaitem_listview.Size = new System.Drawing.Size(432, 583);
            this.schemaitem_listview.TabIndex = 6;
            this.schemaitem_listview.UseCompatibleStateImageBehavior = false;
            this.schemaitem_listview.View = System.Windows.Forms.View.Details;
            // 
            // ColumnDisplayName
            // 
            this.ColumnDisplayName.Text = "Name";
            this.ColumnDisplayName.Width = 300;
            // 
            // ColumnDisplayType
            // 
            this.ColumnDisplayType.Text = "Type";
            this.ColumnDisplayType.Width = 150;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.schemaDefinition_lbl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.GenerateSchemaFileBtn, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(541, 133);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(432, 53);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // schemaDefinition_lbl
            // 
            this.schemaDefinition_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.schemaDefinition_lbl.AutoSize = true;
            this.schemaDefinition_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schemaDefinition_lbl.Location = new System.Drawing.Point(3, 40);
            this.schemaDefinition_lbl.Name = "schemaDefinition_lbl";
            this.schemaDefinition_lbl.Size = new System.Drawing.Size(110, 13);
            this.schemaDefinition_lbl.TabIndex = 0;
            this.schemaDefinition_lbl.Text = "Schema Definition";
            this.schemaDefinition_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GenerateSchemaFileBtn
            // 
            this.GenerateSchemaFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateSchemaFileBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateSchemaFileBtn.Image = global::Dataverse.ConfigurationMigrationTool.XrmToolBox.Properties.Resources.schema;
            this.GenerateSchemaFileBtn.Location = new System.Drawing.Point(223, 14);
            this.GenerateSchemaFileBtn.Name = "GenerateSchemaFileBtn";
            this.GenerateSchemaFileBtn.Size = new System.Drawing.Size(206, 36);
            this.GenerateSchemaFileBtn.TabIndex = 1;
            this.GenerateSchemaFileBtn.Text = "Generate Schema File";
            this.GenerateSchemaFileBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.GenerateSchemaFileBtn.UseVisualStyleBackColor = true;
            this.GenerateSchemaFileBtn.Click += new System.EventHandler(this.GenerateSchemaFileBtn_Click);
            // 
            // informativeRTC
            // 
            this.informativeRTC.BackColor = System.Drawing.SystemColors.Info;
            this.informativeRTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.informativeRTC.Location = new System.Drawing.Point(541, 3);
            this.informativeRTC.Name = "informativeRTC";
            this.informativeRTC.ReadOnly = true;
            this.informativeRTC.ShowSelectionMargin = true;
            this.informativeRTC.Size = new System.Drawing.Size(432, 124);
            this.informativeRTC.TabIndex = 8;
            this.informativeRTC.Text = resources.GetString("informativeRTC.Text");
            // 
            // ConfigurationMigrationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStripMenu);
            this.MinimumSize = new System.Drawing.Size(990, 0);
            this.Name = "ConfigurationMigrationControl";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(990, 835);
            this.TabIcon = global::Dataverse.ConfigurationMigrationTool.XrmToolBox.Properties.Resources.ConfigurationMigrationTool_32x32;
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Schema.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityMetadataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.relationshipm2m_grid)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Schema;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox solutionsCombbox;
        private System.Windows.Forms.ToolStripButton SolutionLoadBtn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox entitylist_cbox;
        private System.Windows.Forms.DataGridView entityMetadataGrid;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label fromselection_lbl;
        private System.Windows.Forms.CheckBox fromentity_select_all_cbox;
        private System.Windows.Forms.DataGridView relationshipm2m_grid;
        private System.Windows.Forms.Button moveRightSchema_btn;
        private System.Windows.Forms.Button moveLeftSchema_btn;
        public System.Windows.Forms.ColumnHeader ColumnDisplayName;
        private System.Windows.Forms.ListView schemaitem_listview;
        private System.Windows.Forms.ColumnHeader ColumnDisplayType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label schemaDefinition_lbl;
        private System.Windows.Forms.Button GenerateSchemaFileBtn;
        private System.Windows.Forms.RichTextBox informativeRTC;
    }
}
