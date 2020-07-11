using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBSystem.BLL;
using DBSystem.ENTITIES;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

namespace WebApp.Exercises
{
    public partial class Exercise08CRUD : System.Web.UI.Page
    {
        static string pid = "";
        static string add = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pid = Request.QueryString["pid"];
                add = Request.QueryString["add"];
                BindGuardianList();
                BindTeamList();
                if (string.IsNullOrEmpty(pid))
                {
                    Response.Redirect("~/Default.aspx");
                }
                else if (add == "yes")
                {
                    UpdateButton.Enabled = false;
                    DeleteButton.Enabled = false;
                    AddButton.Enabled = false;
                }
                else
                {
                    AddButton.Enabled = false;
                    PlayerController sysmgr = new PlayerController();
                    Player info = sysmgr.FindByID2(int.Parse(pid));
                    if (info == null)
                    {
                        ShowMessage("Record is not in Database.", "alert alert-info");
                        Clear(sender, e);
                    }
                    else
                    {
                        PlayerID.Text = info.PlayerID.ToString(); //NOT NULL in Database
                        FirstName.Text = info.FirstandLast; //NOT NULL in Database
                        if (info.GuardianID > 0) //NULL in Database
                        {
                            GuardianList.SelectedValue = info.GuardianID.ToString();
                        }
                        else
                        {
                            TeamList.SelectedValue = "0";
                        }
                        if (info.TeamID > 0) //NULL in Database
                        {
                            TeamList.SelectedValue = info.TeamID.ToString();
                        }
                        else
                        {
                            TeamList.SelectedValue = "0";
                        }
                        FirstName.Text = info.Gender;
                    }
                }
            }
        }

        protected Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
        protected void ShowMessage(string message, string cssclass)
        {
            MessageLabel.Attributes.Add("class", cssclass);
            MessageLabel.InnerHtml = message;
        }
        protected void BindGuardianList()
        {
            try
            {
                GuardianController sysmgr = new GuardianController();
                List<Guardian> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.FirstName.CompareTo(y.FirstName));
                GuardianList.DataSource = info;
                GuardianList.DataTextField = nameof(Guardian.FirstName);
                GuardianList.DataValueField = nameof(Guardian.FirstName);
                GuardianList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                GuardianList.Items.Insert(0, myitem);
                //CategoryList.Items.Insert(0, "select...");

            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }
        protected void BindTeamList()
        {
            try
            {
                TeamController sysmgr = new TeamController();
                List<Team> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.TeamName.CompareTo(y.TeamName));
                TeamList.DataSource = info;
                TeamList.DataTextField = nameof(Supplier.ContactName);
                TeamList.DataValueField = nameof(Supplier.SupplierID);
                TeamList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                TeamList.Items.Insert(0, myitem);
                //SupplierList.Items.Insert(0, "select...");

            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }

        protected bool Validation(object sender, EventArgs e)
        {
            double age = 0;
            if (string.IsNullOrEmpty(FirstName.Text))
            {
                ShowMessage("First Name is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(LastName.Text))
            {
                ShowMessage("Last Name is required", "alert alert-info");
                return false;
            }
            else if (GuardianList.SelectedValue == "0")
            {
                ShowMessage("Guardian is required", "alert alert-info");
                return false;
            }
            else if (TeamList.SelectedValue == "0")
            {
                ShowMessage("Team is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(Gender.Text))
            {
                ShowMessage("Gender is required", "alert alert-info");
                return false;
            }
            else if (string.IsNullOrEmpty(Age.Text))
            {
                ShowMessage("Age is required", "alert alert-info");
                return false;
            }
            else if (double.TryParse(Age.Text, out age))
            {
                ShowMessage("Age must be a real number", "alert alert-info");
                return false;
            }
            else if (int.Parse(Age.Text) > 0 || int.Parse(Age.Text) < 14)
            {
                ShowMessage("Age must be between 0 and 14", "alert alert-info");
                return false;
            }
            return true;

            //if (age < 0 || age > 14)
            //{
            //    ShowMessage("Age must be between 0 and 14", "alert alert-info");
            //    return false;
            //}


        }

        protected void Clear(object sender, EventArgs e)
        {
            PlayerID.Text = "";
            FirstName.Text = "";
            Age.Text = "";
            GuardianList.ClearSelection();
            TeamList.ClearSelection();
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    Player item = new Player();
                    //No ProductID here as the database will give a new one back when we add
                    item.FirstName = FirstName.Text.Trim(); //NOT NULL in Database
                    if (GuardianList.SelectedValue == "0") //NULL in Database
                    {
                        item.GuardianID = null;
                    }
                    else
                    {
                        item.GuardianID = int.Parse(GuardianList.SelectedValue);
                    }
                    //CategoryID can be NULL in Database but NOT NULL when record is added in this CRUD page
                    item.TeamID = int.Parse(TeamList.SelectedValue);
                    //UnitPrice can be NULL in Database but NOT NULL when record is added in this CRUD page
                    item.Age = int.Parse(Age.Text);
                    int newID = sysmgr.Add(item);
                    PlayerID.Text = newID.ToString();
                    ShowMessage("Record has been ADDED", "alert alert-success");
                    AddButton.Enabled = false;
                    UpdateButton.Enabled = true;
                    DeleteButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            var isValid = Validation(sender, e);
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    Player item = new Player();
                    item.PlayerID = int.Parse(PlayerID.Text);
                    item.FirstName = FirstName.Text.Trim();
                    if (GuardianList.SelectedValue == "0")
                    {
                        item.GuardianID = null;
                    }
                    else
                    {
                        item.GuardianID = int.Parse(GuardianList.SelectedValue);
                    }

                    item.Age = int.Parse(Age.Text);
                    int rowsaffected = sysmgr.Update(item);
                    if (rowsaffected > 0)
                    {
                        ShowMessage("Record has been UPDATED", "alert alert-success");
                    }
                    else
                    {
                        ShowMessage("Record was not found", "alert alert-warning");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            var isValid = true;
            if (isValid)
            {
                try
                {
                    PlayerController sysmgr = new PlayerController();
                    int rowsaffected = sysmgr.Delete(int.Parse(PlayerID.Text));
                    if (rowsaffected > 0)
                    {
                        ShowMessage("Record has been DELETED", "alert alert-success");
                        Clear(sender, e);
                    }
                    else
                    {
                        ShowMessage("Record was not found", "alert alert-warning");
                    }
                    UpdateButton.Enabled = false;
                    DeleteButton.Enabled = false;
                    AddButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
    }
}