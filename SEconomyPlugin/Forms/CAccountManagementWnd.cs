/*
 * This file is part of SEconomy - A server-sided currency implementation
 * Copyright (C) 2013-2014, Tyler Watson <tyler@tw.id.au>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading.Tasks;
using TShockAPI;
using Wolfje.Plugins.SEconomy.Journal;

namespace Wolfje.Plugins.SEconomy.Forms {
    public partial class CAccountManagementWnd : Form {
        protected SEconomy SecInstance { get; set; }

        protected IEnumerable<Journal.ITransaction> tranList;

        private List<AccountSummary> sourceAccList = new List<AccountSummary>();
        private BindingList<AccountSummary> accList = new BindingList<AccountSummary>();
        private bool selectionEnabled = true;
        private AccountSummary currentlySelectedAccount = null;

        public CAccountManagementWnd(SEconomy SecInstance)
        {
            InitializeComponent();
            this.SecInstance = SecInstance;
        }

        async Task BindUserList()
        {
            List<Control> disabledControls = new List<Control>();
            DisableAllControls(this, true, ref disabledControls);

            accList.Clear();
            try {
                await Task.Run(() => {
                    foreach (AccountSummary summary in sourceAccList.OrderByDescending(i => (long)i.Balance)) {
                        if ((showOnlineToolStripMenuItem.Checked == true && summary.IsOnline == true)
                              || (showOfflineToolStripMenuItem.Checked == true && summary.IsOnline == false)
                              || (showOrphanNoAccountToolStripMenuItem.Checked == true && summary.HasAccount == false)
                              || (showSystemToolStripMenuItem.Checked == true && summary.IsSystem == true)) {
                            this.MainThreadInvoke(() => accList.Add(summary));
                        }
                    }
                });
            } finally {
                this.MainThreadInvoke(() => EnableAllControls(disabledControls));
            }

            accList.ResetBindings();
        }

        async Task LoadTransactionsForUser(long BankAccountK)
        {
            Journal.IBankAccount selectedAccount = null;
            TSPlayer player;
            List<ITransaction> qTransactions;

            if (selectionEnabled == false) {
                return;
            }

            selectedAccount = SecInstance.RunningJournal.GetBankAccount(BankAccountK);
            qTransactions = selectedAccount.Transactions;

            gvTransactions.DataSource = null;
            gvTransactions.DataSource = qTransactions;
            tranList = SecInstance.RunningJournal.Transactions;

            player = TShock.Players.FirstOrDefault(i => i != null && i.Name == selectedAccount.UserAccountName);

            lblStatus.Text = string.Format("已加载{1}的{0}次交易。", qTransactions.Count(), selectedAccount.UserAccountName);
            if (player != null) {
                lblOnline.Text = "在线";
                lblOnline.ForeColor = System.Drawing.Color.DarkGreen;
            } else {
                lblOnline.Text = "离线";
                lblOnline.ForeColor = System.Drawing.Color.Red;
            }

            await selectedAccount.SyncBalanceAsync();
            AccountSummary summary = accList.FirstOrDefault(i => i.Value == BankAccountK);

            if (summary != null) {
                summary.Balance = selectedAccount.Balance;
            }
            this.MainThreadInvoke(() => {
                lblBalance.Text = selectedAccount.Balance.ToLongString(true);
                lblName.Text = string.Format("{0}, 账户ID{1}", selectedAccount.UserAccountName, selectedAccount.BankAccountK);
            });
        }

        async Task LoadAccountsAsync()
        {
            List<string> tshockAccounts;
            List<Control> disabledControls = new List<Control>();

            int i = 0;
            this.MainThreadInvoke(() => {
                lblStatus.Text = "正在同步账户";
                DisableAllControls(this, true, ref disabledControls);
            });

            sourceAccList.Clear();
            gvAccounts.DataSource = null;

            tshockAccounts = await Task.Run(() => TShock.Users.GetUsers().Select(acc => acc.Name).ToList());

            foreach (Journal.IBankAccount account in SecInstance.RunningJournal.BankAccounts) {
                int p = Convert.ToInt32(((double)i / (double)SecInstance.RunningJournal.BankAccounts.Count) * 100);

                AccountSummary summary = new AccountSummary() {
                    Name = account.UserAccountName,
                    Balance = account.Balance,
                    Value = account.BankAccountK,
                    HasAccount = (account.IsPluginAccount == false || account.IsSystemAccount == false) && tshockAccounts.Any(x => x == account.UserAccountName),
                    IsOnline = (account.IsPluginAccount == true || account.IsSystemAccount == true) || TShock.Players.Any(x => x != null && x.Name == account.UserAccountName),
                    IsSystem = account.IsSystemAccount || account.IsPluginAccount
                };

                sourceAccList.Add(summary);
                tshockAccounts.Remove(account.UserAccountName);

                this.MainThreadInvoke(() => {
                    toolStripProgressBar1.Value = p;
                });

                i++;
            }

            this.MainThreadInvoke(async () => {
                accList = new BindingList<AccountSummary>(sourceAccList.OrderByDescending(x => (long)x.Balance).ToList());
                gvAccounts.DataSource = accList;
                await BindUserList();

                EnableAllControls(disabledControls);

                lblStatus.Text = "同步完成。";
            });
        }


        private async void CAccountManagementWnd_Load(object sender, EventArgs e)
        {
            gvTransactions.AutoGenerateColumns = false;
            gvAccounts.AutoGenerateColumns = false;

            await LoadAccountsAsync();
            this.MainThreadInvoke(() => {
                toolStripProgressBar1.Value = 100;
                lblStatus.Text = "完成。";
            });
        }


        /// <summary>
        /// Occurs when the user selects an item on the left hand menu
        /// </summary>
        private async void lbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountSummary selectedSummary = gvAccounts.SelectedRows[0].DataBoundItem as AccountSummary;
            if (selectedSummary != null) {
                await LoadTransactionsForUser(selectedSummary.Value);
            } else {
                MessageBox.Show("无法加载账户" + selectedSummary.DisplayValue, "错误");
            }
        }

        private void gvTransactions_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var cell = gvTransactions.CurrentCell;
            var cellRect = gvTransactions.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

            if (cell.OwningColumn.HeaderText.Equals("amount", StringComparison.CurrentCultureIgnoreCase)) {
                Money amount = 0;
                if (Money.TryParse((string)cell.Value.ToString(), out amount)) {
                    toolTip1.Show(amount.ToLongString(true), gvTransactions, cellRect.X + cell.Size.Width / 2, cellRect.Y + cell.Size.Height / 2, 10000);
                }
            }

            gvTransactions.ShowCellToolTips = false;
        }


        /// <summary>
        /// Shows the transaction context menu under the mouse cursor
        /// </summary>
        private void gvTransactions_MouseUp(object sender, MouseEventArgs e)
        {
            // Load context menu on right mouse click
            DataGridView.HitTestInfo hitTestInfo;

            if (e.Button == MouseButtons.Right) {
                hitTestInfo = gvTransactions.HitTest(e.X, e.Y);
                // If clicked on a cell
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell) {
                    ctxDeleteTransactionItem.Text = string.Format("删除了{0}次交易。", gvTransactions.SelectedRows.Count);
                    ctxTransaction.Show(gvTransactions, e.X, e.Y);
                }

            }
        }

        /// <summary>
        /// Fires when a user wants to delete some transactions
        /// </summary>
        private async void ctxDeleteTransactionConfirm_Click(object sender, EventArgs e)
        {
            gvTransactions.Enabled = false;
            gvAccounts.Enabled = false;
            btnRefreshTrans.Enabled = false;
            btnResetTrans.Enabled = false;

            await Task.Factory.StartNew(() => {
                int count = gvTransactions.SelectedRows.Count;

                for (int i = 0; i < gvTransactions.SelectedRows.Count; i++) {
                    DataGridViewRow selectedItem = gvTransactions.SelectedRows[i];

                    if (selectedItem.DataBoundItem != null) {
                        Journal.IBankAccount bankAccount;
                        Journal.ITransaction selectedTrans = selectedItem.DataBoundItem as Journal.ITransaction;

                        if (selectedTrans != null) {
                            bankAccount = selectedTrans.BankAccount;

                            //invoke GUI updates on gui thread
                            this.MainThreadInvoke(() => {
                                lblStatus.Text = string.Format("删除了{1}的交易{0}。", selectedTrans.BankAccountTransactionK, ((Money)selectedTrans.Amount).ToLongString(true));
                                toolStripProgressBar1.Value = Convert.ToInt32(((double)i / (double)count) * 100);
                            });

                            bankAccount.Transactions.Remove(selectedTrans);
                        }
                    }
                }
            });

            this.MainThreadInvoke(async () => {
                AccountSummary summary = gvAccounts.SelectedRows[0].DataBoundItem as AccountSummary;
                Journal.IBankAccount selectedUserAccount = SEconomyPlugin.Instance.RunningJournal.BankAccounts.FirstOrDefault(i => i.BankAccountK == summary.Value);

                if (selectedUserAccount != null) {
                    await selectedUserAccount.SyncBalanceAsync();
                    AccountSummary selectedSummary = accList.FirstOrDefault(i => i.Value == selectedUserAccount.BankAccountK);

                    if (selectedSummary != null) {
                        selectedSummary.Balance = selectedUserAccount.Balance;
                    }
                }

                toolStripProgressBar1.Value = 100;
                gvTransactions.Enabled = true;
                gvAccounts.Enabled = true;
                btnRefreshTrans.Enabled = true;
                btnResetTrans.Enabled = true;
                btnShowFrom.Enabled = true;

                await LoadTransactionsForUser(summary.Value);
            });
        }

        /// <summary>
        /// Occurs when a user clicks on the refresh transactions button on the right hand top toolbar
        /// </summary>
        private async void btnRefreshTrans_Click(object sender, EventArgs e)
        {
            if (gvAccounts.SelectedRows.Count > 0) {
                AccountSummary summary = gvAccounts.SelectedRows[0].DataBoundItem as AccountSummary;
                await LoadTransactionsForUser(summary.Value);
            }

        }

        private async void btnResetTrans_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("这将删除该用户的所有交易记录。这将消耗很长时间，并且SEconomy插件的性能将会受到影响。该用户的余额将被设为0。", "删除所有交易", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes) {
                Journal.IBankAccount account = SecInstance.RunningJournal.GetBankAccount(currentlySelectedAccount.Value);

                gvTransactions.Enabled = false;
                gvAccounts.Enabled = false;
                btnRefreshTrans.Enabled = false;
                btnResetTrans.Enabled = false;
                btnShowFrom.Enabled = false;

				lblStatus.Text = "正在删除" + currentlySelectedAccount.Name + "的所有交易。";
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;


                await Task.Factory.StartNew(() => {
                    account.Transactions.Clear();
                });

                Journal.IBankAccount selectedUserAccount = SecInstance.RunningJournal.GetBankAccount(currentlySelectedAccount.Value);
                if (selectedUserAccount != null) {
                    await selectedUserAccount.SyncBalanceAsync();
                    AccountSummary summary = accList.FirstOrDefault(i => i.Value == selectedUserAccount.BankAccountK);

                    if (summary != null) {
                        summary.Balance = selectedUserAccount.Balance;
                    }

                    this.MainThreadInvoke(async () => {
                        toolStripProgressBar1.Value = 100;
                        toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                        gvTransactions.DataSource = null;
                        gvTransactions.Enabled = true;
                        gvAccounts.Enabled = true;
                        btnRefreshTrans.Enabled = true;
                        btnResetTrans.Enabled = true;

                        //force the bindinglist to raise the reset event updating the right hand side
                        accList.ResetBindings();

                        await LoadTransactionsForUser(currentlySelectedAccount.Value);
                    });
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private async void gvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            if (selectionEnabled == false) {
                return;
            }

            if (gvAccounts.SelectedRows.Count > 0) {
                this.currentlySelectedAccount = gvAccounts.SelectedRows[0].DataBoundItem as AccountSummary;


                await LoadTransactionsForUser(this.currentlySelectedAccount.Value);
            } else {
                this.currentlySelectedAccount = null;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0) {
                accList = new BindingList<AccountSummary>(sourceAccList.Where(i => i.Name.ContainsInsensitive(txtSearch.Text)).OrderByDescending(x => (long)x.Balance).ToList());
            } else {
                accList = new BindingList<AccountSummary>(sourceAccList.OrderByDescending(x => (long)x.Balance).ToList());
            }

            gvAccounts.DataSource = accList;
        }

        private async void btnShowFrom_Click(object sender, EventArgs e)
        {
            var vivibleRowsCount = gvTransactions.DisplayedRowCount(true);
            var firstDisplayedRowIndex = gvTransactions.FirstDisplayedCell.RowIndex;
            var lastvibileRowIndex = (firstDisplayedRowIndex + vivibleRowsCount) - 1;

            for (int rowIndex = firstDisplayedRowIndex; rowIndex <= lastvibileRowIndex; rowIndex++) {
                var row = gvTransactions.Rows[rowIndex];
                var cell = row.Cells.OfType<DataGridViewLinkCell>().FirstOrDefault();

                if (string.IsNullOrEmpty(cell.Value as string)) {

                    await Task.Factory.StartNew(() => {
                        Journal.ITransaction trans = row.DataBoundItem as Journal.ITransaction;
                        Journal.ITransaction oppositeTrans = tranList.FirstOrDefault(i => i.BankAccountTransactionK == trans.BankAccountTransactionFK);

                        if (oppositeTrans != null) {
                            AccountSummary oppositeAccount = accList.FirstOrDefault(i => i.Value == oppositeTrans.BankAccountFK);
                            if (oppositeAccount != null) {
                                this.MainThreadInvoke(() => {
                                    cell.Value = oppositeAccount.Name;
                                });
                            }
                        } else {
                            this.MainThreadInvoke(() => {
                                cell.LinkBehavior = LinkBehavior.NeverUnderline;
                                cell.LinkColor = System.Drawing.Color.Gray;

                                cell.Value = "{Unknown}";
                            });
                        }
                    });
                }
            }
        }

        private void EnableAllControls(List<Control> controls)
        {
            if (controls == null
                || controls.Count == 0) {
                return;
            }

            foreach (Control ctrl in controls) {
                ctrl.Enabled = true;
            }
        }

        private void DisableAllControls(Control baseControl, bool recursive, ref List<Control> disabledControls)
        {
            if (baseControl == null || disabledControls == null) {
                return;
            }

            if (baseControl.Enabled == true) {
                disabledControls.Add(baseControl);
                baseControl.Enabled = false;
            }

            if (recursive == false) {
                return;
            }

            foreach (Control childControl in baseControl.Controls) {
                DisableAllControls(childControl, recursive, ref disabledControls);
            }
        }

        private async void btnModifyBalance_Click(object sender, EventArgs e)
        {
            Journal.IBankAccount account = SecInstance.RunningJournal.GetBankAccount(currentlySelectedAccount.Value);
            CModifyBalanceWnd modifyBalanceWindow = new CModifyBalanceWnd(SecInstance, account);

            modifyBalanceWindow.ShowDialog(this);

            //when the dialog has finished, reload the account 
            await LoadTransactionsForUser(account.BankAccountK);
        }

        private void gvTransactions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ctxAccountList_Opening(object sender, CancelEventArgs e)
        {
            int count = 0;
            AccountSummary summary;

            foreach (DataGridViewRow row in gvAccounts.SelectedRows) {
                if ((summary = row.DataBoundItem as AccountSummary) == null
                    || summary.IsSystem == true) {
                    continue;
                }

                count++;
            }

            deleteXAccountsToolStripMenuItem.Enabled = count > 0;
            deleteXAccountsToolStripMenuItem.Text = "删除了" + count + "个账户。";
        }

        private async void deleteXAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountSummary summary;
            List<Control> disabledControls = new List<Control>();
            List<DataGridViewRow> deletedRows = new List<DataGridViewRow>();
            int count = 0;
            int i = 0;

            foreach (DataGridViewRow row in gvAccounts.SelectedRows) {
                if ((summary = row.DataBoundItem as AccountSummary) == null
                    || summary.IsSystem == true) {
                    continue;
                }

                count++;
            }

            if (gvAccounts.SelectedRows.Count == 0
                || MessageBox.Show("确认要删除" + count + "个账户？", "删除账户",
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) {
                return;
            }

            selectionEnabled = false;
            DisableAllControls(this, true, ref disabledControls);

            foreach (DataGridViewRow row in gvAccounts.SelectedRows) {
                this.MainThreadInvoke(() => {
                    this.lblStatus.Text = string.Format("正在删除{0}/{1}", i, count);
                    this.toolStripProgressBar1.Value = (int)((double)i / (double)count * 100);
                });

                if ((summary = row.DataBoundItem as AccountSummary) == null
                || summary.IsSystem == true) {
                    continue;
                }

                /*
                 * Can't delete bank accounts for online users,
                 * reset their transactions instead.
                 */
                if (summary.IsOnline == true) {
                    IBankAccount account = null;
                    if ((account = SEconomyPlugin.Instance.RunningJournal.GetBankAccount(summary.Value)) == null) {
                        continue;
                    }

                    await account.ResetAccountTransactionsAsync(summary.Value);
                    await account.SyncBalanceAsync();
                } else {
                    await SEconomyPlugin.Instance.RunningJournal.DeleteBankAccountAsync(summary.Value);
                }

                i++;
                deletedRows.Add(row);
            }

            foreach (DataGridViewRow row in deletedRows) {
                AccountSummary s;
                if ((s = row.DataBoundItem as AccountSummary) == null
                    || s.IsSystem == true) {
                    continue;
                }

                accList.Remove(s);
            }

            selectionEnabled = true;
            await LoadAccountsAsync();

            EnableAllControls(disabledControls);

            this.MainThreadInvoke(() => {
                this.lblStatus.Text = "完成。";
                this.toolStripProgressBar1.Value = 0;
            });
        }

        private void gvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = gvAccounts.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[0];
            AccountSummary summary = row.DataBoundItem as AccountSummary;

            if (summary.IsSystem == true) {
                cell.Value = "系统";
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(217, 237, 247);
            } else if (summary.IsOnline == true) {
                cell.Value = "在线";
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(223, 240, 216);
            } else if (summary.HasAccount == false) {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 222, 222);
                cell.Value = "无账户";

            } else {
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGray;
                cell.Value = "离线";
            }
        }

        private void showOrphanNoAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindUserList();
        }

        private void showOfflineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindUserList();
        }

        private void showOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindUserList();
        }

        private void showSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindUserList();
        }
	}

	class AccountSummary {
        public string Name { get; set; }

        public long Value { get; set; }

        public Money Balance { get; set; }

        public bool HasAccount { get; set; }

        public bool IsOnline { get; set; }

        public bool IsSystem { get; set; }

        public string DisplayValue
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} [{2}]", this.Name, this.Value, this.Balance);
        }
    }
}
