namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxColumnHeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxColumnProfile = new System.Windows.Forms.TextBox();
            this.profileCatalogColumn = new Tekla.Structures.Dialog.UIControls.ProfileCatalog();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMaterialColumn = new System.Windows.Forms.TextBox();
            this.materialCatalogColumn = new Tekla.Structures.Dialog.UIControls.MaterialCatalog();
            this.splicing = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSpliceMaterial = new System.Windows.Forms.TextBox();
            this.materialCatalogSplice = new Tekla.Structures.Dialog.UIControls.MaterialCatalog();
            this.LSplicing = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSpliceProfile = new System.Windows.Forms.TextBox();
            this.profileCatalogSplice = new Tekla.Structures.Dialog.UIControls.ProfileCatalog();
            this.boltCatalogStandard2 = new Tekla.Structures.Dialog.UIControls.BoltCatalogStandard();
            this.boltCatalogSize1 = new Tekla.Structures.Dialog.UIControls.BoltCatalogSize();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxBoltSize = new System.Windows.Forms.TextBox();
            this.textBoxBoltStandard = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.structuresExtender.SetAttributeName(this.button1, null);
            this.structuresExtender.SetAttributeTypeName(this.button1, null);
            this.structuresExtender.SetBindPropertyName(this.button1, null);
            this.button1.Location = new System.Drawing.Point(82, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create beams";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxColumnHeight
            // 
            this.structuresExtender.SetAttributeName(this.textBoxColumnHeight, "height");
            this.structuresExtender.SetAttributeTypeName(this.textBoxColumnHeight, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxColumnHeight, null);
            this.textBoxColumnHeight.Location = new System.Drawing.Point(72, 24);
            this.textBoxColumnHeight.Name = "textBoxColumnHeight";
            this.textBoxColumnHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxColumnHeight.TabIndex = 1;
            // 
            // label1
            // 
            this.structuresExtender.SetAttributeName(this.label1, null);
            this.structuresExtender.SetAttributeTypeName(this.label1, null);
            this.label1.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label1, null);
            this.label1.Location = new System.Drawing.Point(26, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Height";
            // 
            // label2
            // 
            this.structuresExtender.SetAttributeName(this.label2, null);
            this.structuresExtender.SetAttributeTypeName(this.label2, null);
            this.label2.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label2, null);
            this.label2.Location = new System.Drawing.Point(29, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Profile";
            // 
            // textBoxColumnProfile
            // 
            this.structuresExtender.SetAttributeName(this.textBoxColumnProfile, "Column.Profile");
            this.structuresExtender.SetAttributeTypeName(this.textBoxColumnProfile, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxColumnProfile, null);
            this.textBoxColumnProfile.Location = new System.Drawing.Point(71, 67);
            this.textBoxColumnProfile.Name = "textBoxColumnProfile";
            this.textBoxColumnProfile.Size = new System.Drawing.Size(100, 20);
            this.textBoxColumnProfile.TabIndex = 3;
            // 
            // profileCatalogColumn
            // 
            this.structuresExtender.SetAttributeName(this.profileCatalogColumn, null);
            this.structuresExtender.SetAttributeTypeName(this.profileCatalogColumn, null);
            this.profileCatalogColumn.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.profileCatalogColumn, null);
            this.profileCatalogColumn.ButtonText = "Column profile";
            this.profileCatalogColumn.Location = new System.Drawing.Point(197, 67);
            this.profileCatalogColumn.Name = "profileCatalogColumn";
            this.profileCatalogColumn.SelectedProfile = "";
            this.profileCatalogColumn.Size = new System.Drawing.Size(88, 27);
            this.profileCatalogColumn.TabIndex = 4;
            this.profileCatalogColumn.SelectClicked += new System.EventHandler(this.profileCatalogColumn_SelectClicked);
            this.profileCatalogColumn.SelectionDone += new System.EventHandler(this.profileCatalogColumn_SelectionDone);
            // 
            // label3
            // 
            this.structuresExtender.SetAttributeName(this.label3, null);
            this.structuresExtender.SetAttributeTypeName(this.label3, null);
            this.label3.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label3, null);
            this.label3.Location = new System.Drawing.Point(21, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Material";
            // 
            // textBoxMaterialColumn
            // 
            this.structuresExtender.SetAttributeName(this.textBoxMaterialColumn, "Column.Material");
            this.structuresExtender.SetAttributeTypeName(this.textBoxMaterialColumn, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxMaterialColumn, null);
            this.textBoxMaterialColumn.Location = new System.Drawing.Point(71, 121);
            this.textBoxMaterialColumn.Name = "textBoxMaterialColumn";
            this.textBoxMaterialColumn.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaterialColumn.TabIndex = 6;
            // 
            // materialCatalogColumn
            // 
            this.structuresExtender.SetAttributeName(this.materialCatalogColumn, null);
            this.structuresExtender.SetAttributeTypeName(this.materialCatalogColumn, null);
            this.materialCatalogColumn.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.materialCatalogColumn, null);
            this.materialCatalogColumn.ButtonText = "Column material";
            this.materialCatalogColumn.Location = new System.Drawing.Point(182, 121);
            this.materialCatalogColumn.Name = "materialCatalogColumn";
            this.materialCatalogColumn.SelectedMaterial = "";
            this.materialCatalogColumn.Size = new System.Drawing.Size(103, 27);
            this.materialCatalogColumn.TabIndex = 7;
            this.materialCatalogColumn.SelectClicked += new System.EventHandler(this.materialCatalogColumn_SelectClicked);
            this.materialCatalogColumn.SelectionDone += new System.EventHandler(this.materialCatalogColumn_SelectionDone);
            // 
            // splicing
            // 
            this.structuresExtender.SetAttributeName(this.splicing, null);
            this.structuresExtender.SetAttributeTypeName(this.splicing, null);
            this.structuresExtender.SetBindPropertyName(this.splicing, null);
            this.splicing.Location = new System.Drawing.Point(387, 197);
            this.splicing.Name = "splicing";
            this.splicing.Size = new System.Drawing.Size(131, 43);
            this.splicing.TabIndex = 8;
            this.splicing.Text = "Create rectangular splices";
            this.splicing.UseVisualStyleBackColor = true;
            this.splicing.Click += new System.EventHandler(this.rectangular_splicing_Click);
            // 
            // label4
            // 
            this.structuresExtender.SetAttributeName(this.label4, null);
            this.structuresExtender.SetAttributeTypeName(this.label4, null);
            this.label4.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label4, null);
            this.label4.Location = new System.Drawing.Point(317, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Splice material";
            // 
            // textBoxSpliceMaterial
            // 
            this.structuresExtender.SetAttributeName(this.textBoxSpliceMaterial, "Splice.Material");
            this.structuresExtender.SetAttributeTypeName(this.textBoxSpliceMaterial, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxSpliceMaterial, null);
            this.textBoxSpliceMaterial.Location = new System.Drawing.Point(398, 24);
            this.textBoxSpliceMaterial.Name = "textBoxSpliceMaterial";
            this.textBoxSpliceMaterial.Size = new System.Drawing.Size(100, 20);
            this.textBoxSpliceMaterial.TabIndex = 10;
            // 
            // materialCatalogSplice
            // 
            this.structuresExtender.SetAttributeName(this.materialCatalogSplice, null);
            this.structuresExtender.SetAttributeTypeName(this.materialCatalogSplice, null);
            this.materialCatalogSplice.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.materialCatalogSplice, null);
            this.materialCatalogSplice.ButtonText = "Splice material";
            this.materialCatalogSplice.Location = new System.Drawing.Point(513, 24);
            this.materialCatalogSplice.Name = "materialCatalogSplice";
            this.materialCatalogSplice.SelectedMaterial = "";
            this.materialCatalogSplice.Size = new System.Drawing.Size(103, 27);
            this.materialCatalogSplice.TabIndex = 11;
            this.materialCatalogSplice.SelectClicked += new System.EventHandler(this.materialCatalogSplice_SelectClicked);
            this.materialCatalogSplice.SelectionDone += new System.EventHandler(this.materialCatalogSplice_SelectionDone);
            // 
            // LSplicing
            // 
            this.structuresExtender.SetAttributeName(this.LSplicing, null);
            this.structuresExtender.SetAttributeTypeName(this.LSplicing, null);
            this.structuresExtender.SetBindPropertyName(this.LSplicing, null);
            this.LSplicing.Location = new System.Drawing.Point(387, 260);
            this.LSplicing.Name = "LSplicing";
            this.LSplicing.Size = new System.Drawing.Size(131, 43);
            this.LSplicing.TabIndex = 15;
            this.LSplicing.Text = "Create L splices";
            this.LSplicing.UseVisualStyleBackColor = true;
            this.LSplicing.Click += new System.EventHandler(this.LSplicing_Click);
            // 
            // label6
            // 
            this.structuresExtender.SetAttributeName(this.label6, null);
            this.structuresExtender.SetAttributeTypeName(this.label6, null);
            this.label6.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label6, null);
            this.label6.Location = new System.Drawing.Point(317, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Splice profile";
            // 
            // textBoxSpliceProfile
            // 
            this.structuresExtender.SetAttributeName(this.textBoxSpliceProfile, "Splice.Profile");
            this.structuresExtender.SetAttributeTypeName(this.textBoxSpliceProfile, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxSpliceProfile, null);
            this.textBoxSpliceProfile.Location = new System.Drawing.Point(398, 81);
            this.textBoxSpliceProfile.Name = "textBoxSpliceProfile";
            this.textBoxSpliceProfile.Size = new System.Drawing.Size(100, 20);
            this.textBoxSpliceProfile.TabIndex = 17;
            // 
            // profileCatalogSplice
            // 
            this.structuresExtender.SetAttributeName(this.profileCatalogSplice, null);
            this.structuresExtender.SetAttributeTypeName(this.profileCatalogSplice, null);
            this.profileCatalogSplice.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.profileCatalogSplice, null);
            this.profileCatalogSplice.ButtonText = "Select splice profile";
            this.profileCatalogSplice.Location = new System.Drawing.Point(513, 70);
            this.profileCatalogSplice.Name = "profileCatalogSplice";
            this.profileCatalogSplice.SelectedProfile = "";
            this.profileCatalogSplice.Size = new System.Drawing.Size(103, 45);
            this.profileCatalogSplice.TabIndex = 18;
            this.profileCatalogSplice.SelectClicked += new System.EventHandler(this.profileCatalogSplice_SelectClicked);
            this.profileCatalogSplice.SelectionDone += new System.EventHandler(this.profileCatalogSplice_SelectionDone);
            // 
            // boltCatalogStandard2
            // 
            this.structuresExtender.SetAttributeName(this.boltCatalogStandard2, "ClipBoltStandard");
            this.structuresExtender.SetAttributeTypeName(this.boltCatalogStandard2, "String");
            this.structuresExtender.SetBindPropertyName(this.boltCatalogStandard2, "Text");
            this.boltCatalogStandard2.FormattingEnabled = true;
            this.boltCatalogStandard2.LinkedBoltCatalogSize = this.boltCatalogSize1;
            this.boltCatalogStandard2.Location = new System.Drawing.Point(398, 372);
            this.boltCatalogStandard2.Name = "boltCatalogStandard2";
            this.boltCatalogStandard2.Size = new System.Drawing.Size(121, 21);
            this.boltCatalogStandard2.TabIndex = 19;
            // 
            // boltCatalogSize1
            // 
            this.structuresExtender.SetAttributeName(this.boltCatalogSize1, "ClipBoltSize");
            this.structuresExtender.SetAttributeTypeName(this.boltCatalogSize1, "Distance");
            this.structuresExtender.SetBindPropertyName(this.boltCatalogSize1, "Text");
            this.boltCatalogSize1.FormattingEnabled = true;
            this.boltCatalogSize1.Location = new System.Drawing.Point(398, 332);
            this.boltCatalogSize1.Name = "boltCatalogSize1";
            this.boltCatalogSize1.Size = new System.Drawing.Size(121, 21);
            this.boltCatalogSize1.TabIndex = 20;
            // 
            // label7
            // 
            this.structuresExtender.SetAttributeName(this.label7, null);
            this.structuresExtender.SetAttributeTypeName(this.label7, null);
            this.label7.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label7, null);
            this.label7.Location = new System.Drawing.Point(290, 375);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Choose bolt material";
            // 
            // label8
            // 
            this.structuresExtender.SetAttributeName(this.label8, null);
            this.structuresExtender.SetAttributeTypeName(this.label8, null);
            this.label8.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label8, null);
            this.label8.Location = new System.Drawing.Point(308, 335);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Choose bolt size";
            // 
            // textBoxBoltSize
            // 
            this.structuresExtender.SetAttributeName(this.textBoxBoltSize, "ClipBoltSize");
            this.structuresExtender.SetAttributeTypeName(this.textBoxBoltSize, "Distance");
            this.structuresExtender.SetBindPropertyName(this.textBoxBoltSize, null);
            this.textBoxBoltSize.Location = new System.Drawing.Point(398, 128);
            this.textBoxBoltSize.Name = "textBoxBoltSize";
            this.textBoxBoltSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxBoltSize.TabIndex = 23;
            // 
            // textBoxBoltStandard
            // 
            this.structuresExtender.SetAttributeName(this.textBoxBoltStandard, "ClipBoltStandard");
            this.structuresExtender.SetAttributeTypeName(this.textBoxBoltStandard, "String");
            this.structuresExtender.SetBindPropertyName(this.textBoxBoltStandard, null);
            this.textBoxBoltStandard.Location = new System.Drawing.Point(398, 171);
            this.textBoxBoltStandard.Name = "textBoxBoltStandard";
            this.textBoxBoltStandard.Size = new System.Drawing.Size(100, 20);
            this.textBoxBoltStandard.TabIndex = 24;
            // 
            // label5
            // 
            this.structuresExtender.SetAttributeName(this.label5, null);
            this.structuresExtender.SetAttributeTypeName(this.label5, null);
            this.label5.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label5, null);
            this.label5.Location = new System.Drawing.Point(338, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Bolt size";
            // 
            // label9
            // 
            this.structuresExtender.SetAttributeName(this.label9, null);
            this.structuresExtender.SetAttributeTypeName(this.label9, null);
            this.label9.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label9, null);
            this.label9.Location = new System.Drawing.Point(320, 171);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Bolt material";
            // 
            // Form1
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(639, 482);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxBoltStandard);
            this.Controls.Add(this.textBoxBoltSize);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.boltCatalogSize1);
            this.Controls.Add(this.boltCatalogStandard2);
            this.Controls.Add(this.profileCatalogSplice);
            this.Controls.Add(this.textBoxSpliceProfile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LSplicing);
            this.Controls.Add(this.materialCatalogSplice);
            this.Controls.Add(this.textBoxSpliceMaterial);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.splicing);
            this.Controls.Add(this.materialCatalogColumn);
            this.Controls.Add(this.textBoxMaterialColumn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.profileCatalogColumn);
            this.Controls.Add(this.textBoxColumnProfile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxColumnHeight);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxColumnHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxColumnProfile;
        private Tekla.Structures.Dialog.UIControls.ProfileCatalog profileCatalogColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMaterialColumn;
        private Tekla.Structures.Dialog.UIControls.MaterialCatalog materialCatalogColumn;
        private System.Windows.Forms.Button splicing;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSpliceMaterial;
        private Tekla.Structures.Dialog.UIControls.MaterialCatalog materialCatalogSplice;
        private System.Windows.Forms.Button LSplicing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSpliceProfile;
        private Tekla.Structures.Dialog.UIControls.ProfileCatalog profileCatalogSplice;
        private Tekla.Structures.Dialog.UIControls.BoltCatalogStandard boltCatalogStandard2;
        private Tekla.Structures.Dialog.UIControls.BoltCatalogSize boltCatalogSize1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxBoltSize;
        private System.Windows.Forms.TextBox textBoxBoltStandard;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
    }
}

