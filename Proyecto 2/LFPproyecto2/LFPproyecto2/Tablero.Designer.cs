namespace LFPproyecto2
{
    partial class Tablero
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelJuego = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelJuego
            // 
            this.panelJuego.ColumnCount = 2;
            this.panelJuego.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelJuego.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelJuego.Location = new System.Drawing.Point(0, 42);
            this.panelJuego.Margin = new System.Windows.Forms.Padding(0);
            this.panelJuego.Name = "panelJuego";
            this.panelJuego.RowCount = 4;
            this.panelJuego.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelJuego.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelJuego.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelJuego.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelJuego.Size = new System.Drawing.Size(1126, 540);
            this.panelJuego.TabIndex = 0;
            this.panelJuego.Paint += new System.Windows.Forms.PaintEventHandler(this.panelJuego_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Recorrer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Tablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 481);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelJuego);
            this.Name = "Tablero";
            this.Text = "Tablero";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelJuego;
        private System.Windows.Forms.Button button1;
    }
}