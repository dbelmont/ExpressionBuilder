/*
 * Created by SharpDevelop.
 * User: dcbelmont
 * Date: 18/02/2016
 * Time: 14:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ExpressionBuilder.WinForms.Controls
{
	partial class UcFilter
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox cbProperties;
		private System.Windows.Forms.ComboBox cbOperations;
		private System.Windows.Forms.ComboBox cbConector;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.cbProperties = new System.Windows.Forms.ComboBox();
            this.cbOperations = new System.Windows.Forms.ComboBox();
            this.cbConector = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbProperties
            // 
            this.cbProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProperties.FormattingEnabled = true;
            this.cbProperties.Location = new System.Drawing.Point(4, 4);
            this.cbProperties.Name = "cbProperties";
            this.cbProperties.Size = new System.Drawing.Size(164, 21);
            this.cbProperties.TabIndex = 0;
            // 
            // cbOperations
            // 
            this.cbOperations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperations.FormattingEnabled = true;
            this.cbOperations.Location = new System.Drawing.Point(170, 4);
            this.cbOperations.Name = "cbOperations";
            this.cbOperations.Size = new System.Drawing.Size(145, 21);
            this.cbOperations.TabIndex = 1;
            this.cbOperations.SelectedIndexChanged += new System.EventHandler(this.cbOperations_SelectedIndexChanged);
            // 
            // cbConector
            // 
            this.cbConector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConector.FormattingEnabled = true;
            this.cbConector.Items.AddRange(new object[] {
            "And",
            "Or"});
            this.cbConector.Location = new System.Drawing.Point(775, 4);
            this.cbConector.Name = "cbConector";
            this.cbConector.Size = new System.Drawing.Size(40, 21);
            this.cbConector.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(815, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(25, 19);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(840, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(25, 19);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // ucFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbConector);
            this.Controls.Add(this.cbOperations);
            this.Controls.Add(this.cbProperties);
            this.Name = "ucFilter";
            this.Size = new System.Drawing.Size(868, 27);
            this.Load += new System.EventHandler(this.UcFilterLoad);
            this.ResumeLayout(false);

		}
	}
}
