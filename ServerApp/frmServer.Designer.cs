
namespace ServerApp
{
    partial class frmServer
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNumberOfConnections = new System.Windows.Forms.ToolStripStatusLabel();
            this.rtbServer = new System.Windows.Forms.RichTextBox();
            this.rtbClientConnect = new System.Windows.Forms.RichTextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblNumberOfConnections});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(136, 17);
            this.toolStripStatusLabel1.Text = "Number of connections:";
            // 
            // lblNumberOfConnections
            // 
            this.lblNumberOfConnections.Name = "lblNumberOfConnections";
            this.lblNumberOfConnections.Size = new System.Drawing.Size(13, 17);
            this.lblNumberOfConnections.Text = "0";
            // 
            // rtbServer
            // 
            this.rtbServer.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtbServer.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbServer.Location = new System.Drawing.Point(0, 0);
            this.rtbServer.Name = "rtbServer";
            this.rtbServer.Size = new System.Drawing.Size(800, 204);
            this.rtbServer.TabIndex = 1;
            this.rtbServer.Text = "";
            // 
            // rtbClientConnect
            // 
            this.rtbClientConnect.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtbClientConnect.Location = new System.Drawing.Point(0, 204);
            this.rtbClientConnect.Name = "rtbClientConnect";
            this.rtbClientConnect.Size = new System.Drawing.Size(800, 221);
            this.rtbClientConnect.TabIndex = 2;
            this.rtbClientConnect.Text = "";
            // 
            // frmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbClientConnect);
            this.Controls.Add(this.rtbServer);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmServer";
            this.Text = "Form1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblNumberOfConnections;
        private System.Windows.Forms.RichTextBox rtbServer;
        private System.Windows.Forms.RichTextBox rtbClientConnect;
    }
}

