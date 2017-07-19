using MinorShift.Emuera.Forms;
namespace MinorShift.Emuera
{
	partial class MainWindow
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ログを保存するSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ログをクリップボードにコピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.タイトルへ戻るTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.コードを読み直すcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.フォルダを読み直すFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ファイルを読み直すFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグウインドウを開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグ情報の更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.コンフィグCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EmuVerToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderSelectDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.AutoVerbMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.マクロToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ02ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ03ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ04ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ05ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ06ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ07ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ08ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ09ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクロ12ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.マクログループToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.グループ9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.切り取り = new System.Windows.Forms.ToolStripMenuItem();
            this.コピー = new System.Windows.Forms.ToolStripMenuItem();
            this.貼り付け = new System.Windows.Forms.ToolStripMenuItem();
            this.削除 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.実行 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipButton = new System.Windows.Forms.ToolTip(this.components);
            this.timerKeyMacroChanged = new System.Windows.Forms.Timer(this.components);
            this.labelMacroGroupChanged = new System.Windows.Forms.Label();
            this.mainPicBox = new MinorShift.Emuera.Forms.EraPictureBox();
            this.menuStrip.SuspendLayout();
            this.AutoVerbMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar.Enabled = false;
            this.vScrollBar.LargeChange = 1;
            this.vScrollBar.Location = new System.Drawing.Point(640, 24);
            this.vScrollBar.Maximum = 0;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(18, 480);
            this.vScrollBar.TabIndex = 1;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.デバッグToolStripMenuItem,
            this.ヘルプHToolStripMenuItem,
            this.EmuVerToolStripTextBox});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(657, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rebootToolStripMenuItem,
            this.ログを保存するSToolStripMenuItem,
            this.ログをクリップボードにコピーToolStripMenuItem,
            this.タイトルへ戻るTToolStripMenuItem,
            this.コードを読み直すcToolStripMenuItem,
            this.フォルダを読み直すFToolStripMenuItem,
            this.ファイルを読み直すFToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileToolStripMenuItem.Text = "File(&F)";
            // 
            // rebootToolStripMenuItem
            // 
            this.rebootToolStripMenuItem.Name = "rebootToolStripMenuItem";
            this.rebootToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.rebootToolStripMenuItem.Text = "Restart(&R)";
            this.rebootToolStripMenuItem.Click += new System.EventHandler(this.rebootToolStripMenuItem_Click);
            // 
            // ログを保存するSToolStripMenuItem
            // 
            this.ログを保存するSToolStripMenuItem.Name = "ログを保存するSToolStripMenuItem";
            this.ログを保存するSToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.ログを保存するSToolStripMenuItem.Text = "Save Log...(&S)";
            this.ログを保存するSToolStripMenuItem.Click += new System.EventHandler(this.ログを保存するSToolStripMenuItem_Click);
            // 
            // ログをクリップボードにコピーToolStripMenuItem
            // 
            this.ログをクリップボードにコピーToolStripMenuItem.Name = "ログをクリップボードにコピーToolStripMenuItem";
            this.ログをクリップボードにコピーToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.ログをクリップボードにコピーToolStripMenuItem.Text = "Open Clipboard(&C)";
            this.ログをクリップボードにコピーToolStripMenuItem.Click += new System.EventHandler(this.ログをクリップボードにコピーToolStripMenuItem_Click);
            // 
            // タイトルへ戻るTToolStripMenuItem
            // 
            this.タイトルへ戻るTToolStripMenuItem.Name = "タイトルへ戻るTToolStripMenuItem";
            this.タイトルへ戻るTToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.タイトルへ戻るTToolStripMenuItem.Text = "Return to Title Screen(&T)";
            this.タイトルへ戻るTToolStripMenuItem.Click += new System.EventHandler(this.タイトルへ戻るTToolStripMenuItem_Click);
            // 
            // コードを読み直すcToolStripMenuItem
            // 
            this.コードを読み直すcToolStripMenuItem.Name = "コードを読み直すcToolStripMenuItem";
            this.コードを読み直すcToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.コードを読み直すcToolStripMenuItem.Text = "Reload Code(&C)";
            this.コードを読み直すcToolStripMenuItem.Click += new System.EventHandler(this.コードを読み直すcToolStripMenuItem_Click);
            // 
            // フォルダを読み直すFToolStripMenuItem
            // 
            this.フォルダを読み直すFToolStripMenuItem.Name = "フォルダを読み直すFToolStripMenuItem";
            this.フォルダを読み直すFToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.フォルダを読み直すFToolStripMenuItem.Text = "Reload Folder(&F)";
            this.フォルダを読み直すFToolStripMenuItem.Click += new System.EventHandler(this.フォルダを読み直すFToolStripMenuItem_Click);
            // 
            // ファイルを読み直すFToolStripMenuItem
            // 
            this.ファイルを読み直すFToolStripMenuItem.Name = "ファイルを読み直すFToolStripMenuItem";
            this.ファイルを読み直すFToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.ファイルを読み直すFToolStripMenuItem.Text = "Reload File(&A)";
            this.ファイルを読み直すFToolStripMenuItem.Click += new System.EventHandler(this.ファイルを読み直すFToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.exitToolStripMenuItem.Text = "Close(&X)";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // デバッグToolStripMenuItem
            // 
            this.デバッグToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.デバッグウインドウを開くToolStripMenuItem,
            this.デバッグ情報の更新ToolStripMenuItem});
            this.デバッグToolStripMenuItem.Name = "デバッグToolStripMenuItem";
            this.デバッグToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.デバッグToolStripMenuItem.Text = "Debug(&D)";
            this.デバッグToolStripMenuItem.Visible = false;
            // 
            // デバッグウインドウを開くToolStripMenuItem
            // 
            this.デバッグウインドウを開くToolStripMenuItem.Name = "デバッグウインドウを開くToolStripMenuItem";
            this.デバッグウインドウを開くToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.デバッグウインドウを開くToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.デバッグウインドウを開くToolStripMenuItem.Text = "Open Debug Window";
            this.デバッグウインドウを開くToolStripMenuItem.Click += new System.EventHandler(this.デバッグウインドウを開くToolStripMenuItem_Click);
            // 
            // デバッグ情報の更新ToolStripMenuItem
            // 
            this.デバッグ情報の更新ToolStripMenuItem.Name = "デバッグ情報の更新ToolStripMenuItem";
            this.デバッグ情報の更新ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.デバッグ情報の更新ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.デバッグ情報の更新ToolStripMenuItem.Text = "Update Debug Info";
            this.デバッグ情報の更新ToolStripMenuItem.Click += new System.EventHandler(this.デバッグ情報の更新ToolStripMenuItem_Click);
            // 
            // ヘルプHToolStripMenuItem
            // 
            this.ヘルプHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.コンフィグCToolStripMenuItem});
            this.ヘルプHToolStripMenuItem.Name = "ヘルプHToolStripMenuItem";
            this.ヘルプHToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.ヘルプHToolStripMenuItem.Text = "Help(&H)";
            // 
            // コンフィグCToolStripMenuItem
            // 
            this.コンフィグCToolStripMenuItem.Name = "コンフィグCToolStripMenuItem";
            this.コンフィグCToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.コンフィグCToolStripMenuItem.Text = "Settings(&C)";
            this.コンフィグCToolStripMenuItem.Click += new System.EventHandler(this.コンフィグCToolStripMenuItem_Click);
            // 
            // EmuVerToolStripTextBox
            // 
            this.EmuVerToolStripTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.EmuVerToolStripTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.EmuVerToolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EmuVerToolStripTextBox.Enabled = false;
            this.EmuVerToolStripTextBox.Name = "EmuVerToolStripTextBox";
            this.EmuVerToolStripTextBox.ShortcutsEnabled = false;
            this.EmuVerToolStripTextBox.Size = new System.Drawing.Size(200, 20);
            this.EmuVerToolStripTextBox.Text = "Emuera Ver. 0.000+v00.0";
            this.EmuVerToolStripTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "save99.sav";
            this.openFileDialog.Filter = "save files (save*.sav)|save*.sav|All files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            // 
            // folderSelectDialog
            // 
            this.folderSelectDialog.Description = "Please Select a folder to read";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ContextMenuStrip = this.AutoVerbMenu;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.richTextBox1.Location = new System.Drawing.Point(0, 486);
            this.richTextBox1.MaxLength = 32767;
            this.richTextBox1.Multiline = false;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(640, 18);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            // 
            // AutoVerbMenu
            // 
            this.AutoVerbMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.マクロToolStripMenuItem,
            this.マクログループToolStripMenuItem,
            this.toolStripSeparator1,
            this.切り取り,
            this.コピー,
            this.貼り付け,
            this.削除,
            this.toolStripSeparator2,
            this.実行});
            this.AutoVerbMenu.Name = "AutoVerbMenu";
            this.AutoVerbMenu.ShowImageMargin = false;
            this.AutoVerbMenu.Size = new System.Drawing.Size(120, 170);
            this.AutoVerbMenu.Opened += new System.EventHandler(this.AutoVerbMenu_Opened);
            // 
            // マクロToolStripMenuItem
            // 
            this.マクロToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.マクロ01ToolStripMenuItem,
            this.マクロ02ToolStripMenuItem,
            this.マクロ03ToolStripMenuItem,
            this.マクロ04ToolStripMenuItem,
            this.マクロ05ToolStripMenuItem,
            this.マクロ06ToolStripMenuItem,
            this.マクロ07ToolStripMenuItem,
            this.マクロ08ToolStripMenuItem,
            this.マクロ09ToolStripMenuItem,
            this.マクロ10ToolStripMenuItem,
            this.マクロ11ToolStripMenuItem,
            this.マクロ12ToolStripMenuItem});
            this.マクロToolStripMenuItem.Name = "マクロToolStripMenuItem";
            this.マクロToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.マクロToolStripMenuItem.Text = "Macro";
            // 
            // マクロ01ToolStripMenuItem
            // 
            this.マクロ01ToolStripMenuItem.Name = "マクロ01ToolStripMenuItem";
            this.マクロ01ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.マクロ01ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ01ToolStripMenuItem.Tag = "";
            this.マクロ01ToolStripMenuItem.Text = "Macro 01";
            // 
            // マクロ02ToolStripMenuItem
            // 
            this.マクロ02ToolStripMenuItem.Name = "マクロ02ToolStripMenuItem";
            this.マクロ02ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.マクロ02ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ02ToolStripMenuItem.Text = "Macro 02";
            // 
            // マクロ03ToolStripMenuItem
            // 
            this.マクロ03ToolStripMenuItem.Name = "マクロ03ToolStripMenuItem";
            this.マクロ03ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.マクロ03ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ03ToolStripMenuItem.Text = "Macro 03";
            // 
            // マクロ04ToolStripMenuItem
            // 
            this.マクロ04ToolStripMenuItem.Name = "マクロ04ToolStripMenuItem";
            this.マクロ04ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.マクロ04ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ04ToolStripMenuItem.Text = "Macro 04";
            // 
            // マクロ05ToolStripMenuItem
            // 
            this.マクロ05ToolStripMenuItem.Name = "マクロ05ToolStripMenuItem";
            this.マクロ05ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.マクロ05ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ05ToolStripMenuItem.Text = "Macro 05";
            // 
            // マクロ06ToolStripMenuItem
            // 
            this.マクロ06ToolStripMenuItem.Name = "マクロ06ToolStripMenuItem";
            this.マクロ06ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.マクロ06ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ06ToolStripMenuItem.Text = "Macro 06";
            // 
            // マクロ07ToolStripMenuItem
            // 
            this.マクロ07ToolStripMenuItem.Name = "マクロ07ToolStripMenuItem";
            this.マクロ07ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.マクロ07ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ07ToolStripMenuItem.Text = "Macro 07";
            // 
            // マクロ08ToolStripMenuItem
            // 
            this.マクロ08ToolStripMenuItem.Name = "マクロ08ToolStripMenuItem";
            this.マクロ08ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.マクロ08ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ08ToolStripMenuItem.Text = "Macro 08";
            // 
            // マクロ09ToolStripMenuItem
            // 
            this.マクロ09ToolStripMenuItem.Name = "マクロ09ToolStripMenuItem";
            this.マクロ09ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.マクロ09ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ09ToolStripMenuItem.Text = "Macro 09";
            // 
            // マクロ10ToolStripMenuItem
            // 
            this.マクロ10ToolStripMenuItem.Name = "マクロ10ToolStripMenuItem";
            this.マクロ10ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.マクロ10ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ10ToolStripMenuItem.Text = "Macro 10";
            // 
            // マクロ11ToolStripMenuItem
            // 
            this.マクロ11ToolStripMenuItem.Name = "マクロ11ToolStripMenuItem";
            this.マクロ11ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.マクロ11ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ11ToolStripMenuItem.Text = "Macro 11";
            // 
            // マクロ12ToolStripMenuItem
            // 
            this.マクロ12ToolStripMenuItem.Name = "マクロ12ToolStripMenuItem";
            this.マクロ12ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.マクロ12ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.マクロ12ToolStripMenuItem.Text = "Macro 12";
            // 
            // マクログループToolStripMenuItem
            // 
            this.マクログループToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.グループ0ToolStripMenuItem,
            this.グループ1ToolStripMenuItem,
            this.グループ2ToolStripMenuItem,
            this.グループ3ToolStripMenuItem,
            this.グループ4ToolStripMenuItem,
            this.グループ5ToolStripMenuItem,
            this.グループ6ToolStripMenuItem,
            this.グループ7ToolStripMenuItem,
            this.グループ8ToolStripMenuItem,
            this.グループ9ToolStripMenuItem});
            this.マクログループToolStripMenuItem.Name = "マクログループToolStripMenuItem";
            this.マクログループToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.マクログループToolStripMenuItem.Text = "Macro Group";
            // 
            // グループ0ToolStripMenuItem
            // 
            this.グループ0ToolStripMenuItem.Name = "グループ0ToolStripMenuItem";
            this.グループ0ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ0ToolStripMenuItem.Tag = "0";
            this.グループ0ToolStripMenuItem.Text = "Group 0";
            this.グループ0ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ1ToolStripMenuItem
            // 
            this.グループ1ToolStripMenuItem.Name = "グループ1ToolStripMenuItem";
            this.グループ1ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ1ToolStripMenuItem.Tag = "1";
            this.グループ1ToolStripMenuItem.Text = "Group 1";
            this.グループ1ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ2ToolStripMenuItem
            // 
            this.グループ2ToolStripMenuItem.Name = "グループ2ToolStripMenuItem";
            this.グループ2ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ2ToolStripMenuItem.Tag = "2";
            this.グループ2ToolStripMenuItem.Text = "Group 2";
            this.グループ2ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ3ToolStripMenuItem
            // 
            this.グループ3ToolStripMenuItem.Name = "グループ3ToolStripMenuItem";
            this.グループ3ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ3ToolStripMenuItem.Tag = "3";
            this.グループ3ToolStripMenuItem.Text = "Group 3";
            this.グループ3ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ4ToolStripMenuItem
            // 
            this.グループ4ToolStripMenuItem.Name = "グループ4ToolStripMenuItem";
            this.グループ4ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ4ToolStripMenuItem.Tag = "4";
            this.グループ4ToolStripMenuItem.Text = "Group 4";
            this.グループ4ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ5ToolStripMenuItem
            // 
            this.グループ5ToolStripMenuItem.Name = "グループ5ToolStripMenuItem";
            this.グループ5ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ5ToolStripMenuItem.Tag = "5";
            this.グループ5ToolStripMenuItem.Text = "Group 5";
            this.グループ5ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ6ToolStripMenuItem
            // 
            this.グループ6ToolStripMenuItem.Name = "グループ6ToolStripMenuItem";
            this.グループ6ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ6ToolStripMenuItem.Tag = "6";
            this.グループ6ToolStripMenuItem.Text = "Group 6";
            this.グループ6ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ7ToolStripMenuItem
            // 
            this.グループ7ToolStripMenuItem.Name = "グループ7ToolStripMenuItem";
            this.グループ7ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ7ToolStripMenuItem.Tag = "7";
            this.グループ7ToolStripMenuItem.Text = "Group 7";
            this.グループ7ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ8ToolStripMenuItem
            // 
            this.グループ8ToolStripMenuItem.Name = "グループ8ToolStripMenuItem";
            this.グループ8ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ8ToolStripMenuItem.Tag = "8";
            this.グループ8ToolStripMenuItem.Text = "Group 8";
            this.グループ8ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // グループ9ToolStripMenuItem
            // 
            this.グループ9ToolStripMenuItem.Name = "グループ9ToolStripMenuItem";
            this.グループ9ToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.グループ9ToolStripMenuItem.Tag = "9";
            this.グループ9ToolStripMenuItem.Text = "Group 9";
            this.グループ9ToolStripMenuItem.Click += new System.EventHandler(this.グループToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(116, 6);
            // 
            // 切り取り
            // 
            this.切り取り.Enabled = false;
            this.切り取り.Name = "切り取り";
            this.切り取り.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.切り取り.Size = new System.Drawing.Size(119, 22);
            this.切り取り.Text = "Cut";
            this.切り取り.Click += new System.EventHandler(this.切り取り_Click);
            // 
            // コピー
            // 
            this.コピー.Enabled = false;
            this.コピー.Name = "コピー";
            this.コピー.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.コピー.Size = new System.Drawing.Size(119, 22);
            this.コピー.Text = "Copy";
            this.コピー.Click += new System.EventHandler(this.コピー_Click);
            // 
            // 貼り付け
            // 
            this.貼り付け.Enabled = false;
            this.貼り付け.Name = "貼り付け";
            this.貼り付け.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.貼り付け.Size = new System.Drawing.Size(119, 22);
            this.貼り付け.Text = "Paste";
            this.貼り付け.Click += new System.EventHandler(this.貼り付け_Click);
            // 
            // 削除
            // 
            this.削除.Enabled = false;
            this.削除.Name = "削除";
            this.削除.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.削除.Size = new System.Drawing.Size(119, 22);
            this.削除.Text = "Delete";
            this.削除.Click += new System.EventHandler(this.削除_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(116, 6);
            // 
            // 実行
            // 
            this.実行.Enabled = false;
            this.実行.Name = "実行";
            this.実行.Size = new System.Drawing.Size(119, 22);
            this.実行.Text = "Execute";
            this.実行.Click += new System.EventHandler(this.実行_Click);
            //
            // toolTipButton
            //
            this.toolTipButton.AutoPopDelay = 30000;
            this.toolTipButton.InitialDelay = 500;
            this.toolTipButton.ReshowDelay = 100;
            // 
            // timerKeyMacroChanged
            // 
            this.timerKeyMacroChanged.Tick += new System.EventHandler(this.timerKeyMacroChanged_Tick);
            // 
            // labelMacroGroupChanged
            // 
            this.labelMacroGroupChanged.AutoSize = true;
            this.labelMacroGroupChanged.BackColor = System.Drawing.Color.Black;
            this.labelMacroGroupChanged.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMacroGroupChanged.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMacroGroupChanged.Location = new System.Drawing.Point(0, 448);
            this.labelMacroGroupChanged.Name = "labelMacroGroupChanged";
            this.labelMacroGroupChanged.Size = new System.Drawing.Size(94, 35);
            this.labelMacroGroupChanged.TabIndex = 5;
            this.labelMacroGroupChanged.Text = "label1";
            this.labelMacroGroupChanged.Visible = false;
            // 
            // mainPicBox
            // 
            this.mainPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPicBox.BackColor = System.Drawing.Color.Black;
            this.mainPicBox.Location = new System.Drawing.Point(0, 24);
            this.mainPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.mainPicBox.Name = "mainPicBox";
            this.mainPicBox.Size = new System.Drawing.Size(640, 480);
            this.mainPicBox.TabIndex = 0;
            this.mainPicBox.TabStop = false;
            this.mainPicBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPicBox_Paint);
            this.mainPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPicBox_MouseDown);
            this.mainPicBox.MouseLeave += new System.EventHandler(this.mainPicBox_MouseLeave);
            this.mainPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPicBox_MouseMove);
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(657, 504);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.labelMacroGroupChanged);
            this.Controls.Add(this.mainPicBox);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Emuera";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.AutoVerbMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.VScrollBar vScrollBar;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rebootToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripMenuItem ヘルプHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem コンフィグCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem タイトルへ戻るTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem コードを読み直すcToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ログを保存するSToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private EraPictureBox mainPicBox;
        private System.Windows.Forms.ToolStripMenuItem ログをクリップボードにコピーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ファイルを読み直すFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem フォルダを読み直すFToolStripMenuItem;
		private System.Windows.Forms.FolderBrowserDialog folderSelectDialog;
        private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ToolStripMenuItem デバッグToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem デバッグウインドウを開くToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem デバッグ情報の更新ToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip AutoVerbMenu;
		private System.Windows.Forms.ToolStripMenuItem 切り取り;
		private System.Windows.Forms.ToolStripMenuItem コピー;
		private System.Windows.Forms.ToolStripMenuItem 貼り付け;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 実行;
		private System.Windows.Forms.ToolStripMenuItem マクロToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ01ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem マクロ02ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ03ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ04ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ05ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ06ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ07ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ08ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ09ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ10ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ11ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem マクロ12ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 削除;
		private System.Windows.Forms.ToolTip toolTipButton;
		private System.Windows.Forms.Timer timerKeyMacroChanged;
		private System.Windows.Forms.Label labelMacroGroupChanged;
		private System.Windows.Forms.ToolStripMenuItem マクログループToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ0ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ3ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ4ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ5ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ6ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ7ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ8ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem グループ9ToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox EmuVerToolStripTextBox;
	}
}

