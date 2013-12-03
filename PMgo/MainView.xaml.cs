using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace PMgo
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : Window
	{
		public MainView(string name)
		{
			InitializeComponent();
			this.ProjectNameValue = name;
			this.projectNameField.Text = name;
			//fillProgressBar();
			PopulateTreeView();
            expandTreeView(ProjectTreeView);
            //verification();
		   
		}
		string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

		//private string _theValue;

		public string ProjectNameValue
		{
			get;
			set;
		}

               
        string _theUser;
  
        public string UserValue
        {
            get { return _theUser; } 
            set 
            {
                _theUser = value;
                this.current_txt.Text = _theUser;
                verification();

            }

        }

        void verification()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
			try
			{
				conn.Open();
				string query = "select user_name from users join projects on (projects.proj_mgr = users.id) where projects.project_name = '" + this.projectNameField.Text + "' and users.id = projects.proj_mgr;";
				SQLiteCommand createCommand = new SQLiteCommand(query, conn);				
				SQLiteDataReader dr = createCommand.ExecuteReader();
                
				while (dr.Read())
				{
                    string manager = dr.GetString(0);
                    //MessageBox.Show(manager);
                    //MessageBox.Show(this.current_txt.Text);
                    if (manager != this.current_txt.Text)
                    {
                        cancelProjectButton.Visibility = Visibility.Hidden;
                        manageMembersButton.Visibility = Visibility.Hidden;
                    }
				}
            }
            catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

        }
		void fillProgressBar()
		{
            string Text1;
 
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
			try
			{
				conn.Open();
				//string query = "select * from projects where project_name = '" + this.projectNameField.Text + "';";

				//string query = "select * from tasks";
				string query = "select * from tasks where tasks.proj_id = (select project_id from projects where project_name = '" + ProjectNameValue + "');";
				SQLiteCommand createCommand = new SQLiteCommand(query, conn);
				//MessageBox.Show(ProjectNameValue);
				//createCommand.ExecuteNonQuery();
				SQLiteDataReader dr = createCommand.ExecuteReader();
				int totalTasks = 0;
				while (dr.Read())
				{
					totalTasks = totalTasks + 1;
				}


				String query2 = "select * from tasks where tasks.proj_id = (select project_id from projects where project_name = '" + this.projectNameField.Text + "') and complete = 1;";
				SQLiteCommand createCommand2 = new SQLiteCommand(query2, conn);
				SQLiteDataReader dr2 = createCommand2.ExecuteReader();
				int completedTasks = 0;
				while (dr2.Read())
				{
					completedTasks = completedTasks + 1;
				}
				//MessageBox.Show(totalTasks + "  " + completedTasks);
				if (totalTasks == 0 && completedTasks == 0)
				{
					projectProgress.Maximum = 1;
					projectProgress.Value = 0;
				}
				else
				{
					projectProgress.Maximum = totalTasks;
					projectProgress.Value = completedTasks;
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
           
            int percent = (int)(((double)(projectProgress.Value - projectProgress.Minimum) /
                   (double)(projectProgress.Maximum - projectProgress.Minimum)) * 100);
            Text1 = percent.ToString() + "%";
            progressBar_txt.Text = Text1;
            
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
            String projectName = this.projectNameField.Text;
            String userName = this.current_txt.Text;
            addUser user = new addUser();
            user.ProjectNameValue = projectName;
            user.UserValue = userName;
			user.ShowDialog();
			this.Close();
		}

		private void projectProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

		}

		private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{

		}

	   
		private void DataView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{

		}


		/// <summary>
		/// Populates the TreeView.
		/// </summary>
		public void PopulateTreeView()
		{
			ProjectTreeView.Items.Clear();
			List<ProjectItem> Milestones = new List<ProjectItem>();
			List<ProjectItem> Tasks;
			List<ProjectItem> Subtasks;
			TreeViewItem currentMilestone;
			TreeViewItem currentTask;
			TreeViewItem currentSubtask;
			SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
			try
			{
				conn.Open();

				int milestoneProgressMaximum = 0;
				int milestoneProgressValue = 0;

				#region Milestones
				//pull milestones from the current project
				string MilestonesQuery = "select * from milestones where milestones.project_id = (select project_id from projects where project_name = '" + ProjectNameValue + "');";
				SQLiteCommand createMilestonesCommand = new SQLiteCommand(MilestonesQuery, conn);
				SQLiteDataReader MilestonesRead = createMilestonesCommand.ExecuteReader();
				while (MilestonesRead.Read())
				{
					int id = (int)(MilestonesRead.GetDouble(0));
					string name = (string)MilestonesRead.GetString(1);
					string desc = (string)MilestonesRead.GetString(2);
					bool complete = (bool)MilestonesRead.GetFieldValue<bool>(5);
					Milestones.Add(new ProjectItem(id, ProjectItemType.MILESTONE, name, desc, complete));
				}

				//loop through the pulled milestones
				for (int i = 0; i < Milestones.Count; i++)
				{
					currentMilestone = new TreeViewItem();
					int taskProgressMaximum = 0;
					int taskProgressValue = 0;

					#region Tasks
					//pull tasks for the current milestone from the database
					Tasks = new List<ProjectItem>();

					string TasksQuery = "select * from tasks where tasks.milestone_id = (select milestone_id from milestones where milestone_id = '" + Milestones[i].id + "');";
					SQLiteCommand createTasksCommand = new SQLiteCommand(TasksQuery, conn);
					SQLiteDataReader TasksRead = createTasksCommand.ExecuteReader();
					while (TasksRead.Read())
					{
						int tid = (int)TasksRead.GetDouble(0);
						string tname = (string)TasksRead.GetString(2);
						string tdesc = (string)TasksRead.GetString(3);

						bool tcomplete = (int)TasksRead.GetDouble(6) == 1;
						Tasks.Add(new ProjectItem(tid, ProjectItemType.TASK, tname, tdesc, tcomplete));
					}


					//loop through the pulled tasks
					for (int j = 0; j < Tasks.Count; j++)
					{
						currentTask = new TreeViewItem();
						int subProgressMaximum = 0;
						int subProgressValue = 0;

						#region Subtasks
						//pull subtasks for the current task
						Subtasks = new List<ProjectItem>();
						string SubtasksQuery = "select * from subtasks where subtasks.task_id = (select task_id from tasks where task_id = '" + Tasks[j].id + "');";
						SQLiteCommand createSubtasksCommand = new SQLiteCommand(SubtasksQuery, conn);
						SQLiteDataReader SubtasksRead = createSubtasksCommand.ExecuteReader();
						while (SubtasksRead.Read())
						{
							int id = (int)(SubtasksRead.GetDouble(0));
							string name = (string)SubtasksRead.GetString(2);
							string desc = (string)SubtasksRead.GetString(3);
							bool complete = (bool)SubtasksRead.GetFieldValue<bool>(6);
							Subtasks.Add(new ProjectItem(id, ProjectItemType.SUBTASK, name, desc, complete));
						}

						//loop through the subtasks
						for (int k = 0; k < Subtasks.Count; k++)
						{
							//reset the currentSubtask object
							currentSubtask = new TreeViewItem();




							#region Add Subtask to Tree

							//create a new StackPanel, set its Orientation to Horizontal, give it a little bit of a margin
							//to separate it from other items
							currentSubtask.Margin = new Thickness(0, 10, 0, 0);
							currentSubtask.Tag = Subtasks[k];
							StackPanel subHeader = new StackPanel();
							subHeader.Orientation = Orientation.Horizontal;

							//create a TextBlock to hold the subtask's name
							//I gave them a standard Width instead of having them autosize to the text
							//because otherwise it messed up the ProgressBars
							TextBlock subName = new TextBlock();
							subName.Text = Subtasks[k].name;
							subName.Width = 200;

							//create a ProgressBar
							//Set its Width
							//Since a subtask doesn't have anything under it, we can automatically set its Maximum to 1;
							ProgressBar subProgress = new ProgressBar();
							subProgress.Width = 150;
                            subProgress.Foreground = Brushes.SkyBlue;
							subProgress.Maximum = 1;

							//If the current Subtask is complete
							//Set the ProgressBar's Value to 1
							//Otherwise set it to 0
							if (Subtasks[k].complete)
							{
								subProgress.Value = 1;
							}
							else
							{
								subProgress.Value = 0;
							}

							//Add the TextBlock and ProgressBar to the StackPanel
							subHeader.Children.Add(subName);
							subHeader.Children.Add(subProgress);

							//Set the header to the StackPanel
							currentSubtask.Header = subHeader;

							//Add the currentSubtask to the currentTask
							currentTask.Items.Add(currentSubtask);

							//The currentTask's Maximum is set to the sum of all of the subtaskMaximums
							//Same with the Value
							//So we need to upsate them both after each subtask is added;
							subProgressMaximum = subProgressMaximum + (int)subProgress.Maximum;
							subProgressValue = subProgressValue + (int)subProgress.Value;

							#endregion

						#endregion
						}

						#region Add Task to Tree


						currentTask.Margin = new Thickness(0, 10, 0, 0);
						currentTask.Tag = Tasks[j];
						StackPanel taskHeader = new StackPanel();
						taskHeader.Orientation = Orientation.Horizontal;

						TextBlock taskName = new TextBlock();
						taskName.Text = Tasks[j].name;
						taskName.Width = 200;

						ProgressBar taskProgress = new ProgressBar();
						taskProgress.Width = 150;
                        taskProgress.Foreground = Brushes.SteelBlue;
						if (Subtasks.Count > 0)
						{
							taskProgress.Maximum = subProgressMaximum;
							taskProgress.Value = subProgressValue;
						}
						else
						{
							taskProgress.Maximum = 1;

							if (Tasks[j].complete)
							{
								taskProgress.Value = 1;
							}
							else
							{
								taskProgress.Value = 0;
							}
						}

						taskHeader.Children.Add(taskName);
						taskHeader.Children.Add(taskProgress);

						currentTask.Header = taskHeader;


						currentMilestone.Items.Add(currentTask);
						#endregion


						taskProgressValue = taskProgressValue + (int)taskProgress.Value;
						taskProgressMaximum = taskProgressMaximum + (int)taskProgress.Maximum;

					#endregion
					}


					#region Add Milestone to Tree


					currentMilestone.Margin = new Thickness(0, 10, 0, 0);
					currentMilestone.Tag = Milestones[i];
					StackPanel milestoneHeader = new StackPanel();
					milestoneHeader.Orientation = Orientation.Horizontal;

					TextBlock milestoneName = new TextBlock();
					milestoneName.Text = Milestones[i].name;
					milestoneName.Width = 200;

					ProgressBar milestoneProgress = new ProgressBar();
					milestoneProgress.Width = 150;
                    milestoneProgress.Foreground = Brushes.MidnightBlue;
					if (Tasks.Count > 0)
					{
						milestoneProgress.Maximum = taskProgressMaximum;
						milestoneProgress.Value = taskProgressValue;
					}
					else
					{
						milestoneProgress.Maximum = 1;

						if (Milestones[i].complete)
						{
							milestoneProgress.Value = 1;
						}
						else
						{
							milestoneProgress.Value = 0;
						}
					}

					milestoneHeader.Children.Add(milestoneName);
					milestoneHeader.Children.Add(milestoneProgress);

					currentMilestone.Header = milestoneHeader;


					ProjectTreeView.Items.Add(currentMilestone);

					milestoneProgressMaximum = milestoneProgressMaximum + (int)milestoneProgress.Maximum;
					milestoneProgressValue = milestoneProgressValue + (int)milestoneProgress.Value;
					#endregion
				}
				projectProgress.Maximum = milestoneProgressMaximum;
				projectProgress.Value = milestoneProgressValue;
				#endregion

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (conn.State == System.Data.ConnectionState.Open)
					conn.Close();
			}
            int percent = 0;
            String Text1;
            percent = (int)(((double)(projectProgress.Value - projectProgress.Minimum) /
                (double)(projectProgress.Maximum - projectProgress.Minimum)) * 100);
            Text1 = percent.ToString() + "% Complete";
            progressBar_txt.Text = Text1;
            //progressBar_txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;

		}

        void expandTreeView(TreeView tv)
        {
            ItemCollection ic = tv.Items;

            foreach (TreeViewItem item in ic)
            {
                item.IsExpanded = true;
                
            }
        }

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
		   
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			String project_name = this.projectNameField.Text;
            String user_name = this.current_txt.Text;
			//MessageBox.Show(project_name);
            itemWindow items = new itemWindow();
			items.ProjectNameValue = project_name;
            items.UserValue = user_name;
			items.ShowDialog();
			
		}

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //fillProgressBar();
            PopulateTreeView();
            expandTreeView(ProjectTreeView);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String project_name = this.projectNameField.Text;
            //MessageBox.Show(project_name);

            String user = this.current_txt.Text;            
            itemWindow itemWin = new itemWindow();
            itemWin.ProjectNameValue = project_name;
            itemWin.UserValue = user;
            itemWin.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this project?", "Cancel Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    conn.Open();
                    string query = "delete from projects where project_name = '" + this.projectNameField.Text + "'";

                    SQLiteCommand createcommand = new SQLiteCommand(query, conn);
                    createcommand.ExecuteNonQuery();
                    MessageBox.Show("Project has been canceled!");

                    conn.Close();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
 
	}


	enum ProjectItemType {MILESTONE, TASK, SUBTASK}
	class ProjectItem
	{
		public int id { get; set; }
		public String name { get; set; }
		public String description { get; set; }
		public bool complete { get; set; }
		public ProjectItemType Type { get; set; }

		public ProjectItem(int i, ProjectItemType t, String n, String d, bool c)
		{
			this.id = i;
			this.name = n;
			this.description = d;
			this.complete = c;
			this.Type = t;
		}
	}



}
