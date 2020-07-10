using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBSystem.ENTITIES;
using DBSystem.BLL;

namespace WebApp.Exercises
{
    public partial class Exercise06 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TeamID.Text = "";
            TeamName.Text = "";
        }

        protected void Fetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IDArg.Text))
            {
                MessageLabel.Text = "Enter a Team ID .";
                TeamID.Text = "";
                TeamName.Text = "";
            }
            else
            {
                int id = 0;
                if (int.TryParse(IDArg.Text, out id))
                {
                    if (id > 0)
                    {
                        TeamController sysmgr = new TeamController();
                        Team info = null;
                        info = sysmgr.FindByPKID(id); 
                        if (info == null)
                        {
                            MessageLabel.Text = "ID not found.";
                            TeamID.Text = "";
                            TeamName.Text = "";
                        }
                        else
                        {
                            TeamID.Text = info.TeamID.ToString();
                            TeamName.Text = info.TeamName;
                        }
                    }
                    else
                    {
                        MessageLabel.Text = "ID must be greater than 0";
                        TeamID.Text = "";
                        TeamName.Text = "";
                    }

                }
                else
                {
                    MessageLabel.Text = "ID must be a number.";
                    TeamID.Text = "";
                    TeamName.Text = "";
                }
            }
        }
    }
}