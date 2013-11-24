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
using System.Data;

namespace PMgo
{
    /// <summary>
    /// Interaction logic for addMenu.xaml
    /// </summary>
    public partial class itemWindow : Window
    {
        string dbConnectionString = "Data Source=PMgo.sqlite;Version=3;";

        public itemWindow()
        {
            InitializeComponent();            
            this.ProjectNameValue = this.projNameBox.Text;
            expandTreeView(ItemTreeView);
            
        }

        string _theValue;
        public string ProjectNameValue
        {
            get { return _theValue; }
            set
            {
                _theValue = value;
                this.projNameBox.Text = _theValue;
                PopulateTreeView();
                expandTreeView(ItemTreeView);
            }

        }
        

        void clearInputItems()
        {
            nameBox.Text = String.Empty;
            descriptionBox.Text = String.Empty;
            startBox.Text = String.Empty;
            endBox.Text = String.Empty;
            //parent_combo.Items.Clear();
            
        }    
        

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();  
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CreateComboBox_Initialized(object sender, EventArgs e)
        {
            CreateComboBox.SelectedValue = CreateComboBox.Items[0];
        }

        private void PopulateTreeView()
        {
            ItemTreeView.Items.Clear();
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


					ItemTreeView.Items.Add(currentMilestone);

					milestoneProgressMaximum = milestoneProgressMaximum + (int)milestoneProgress.Maximum;
					milestoneProgressValue = milestoneProgressValue + (int)milestoneProgress.Value;
					#endregion
				}
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

        }

        void expandTreeView(TreeView tv)
        {
            ItemCollection ic = tv.Items;
            
            foreach (TreeViewItem item in ic)
            {
                item.IsExpanded = true;
                                
            }
        }

        

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            //PopulateTreeView();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {
                TreeViewItem item = ItemTreeView.SelectedItem as TreeViewItem;
				ProjectItem projectItem = item.Tag as ProjectItem;
				String table = "";
				int nameIndex = 0, descIndex = 0 , startIndex = 0, endIndex = 0;


				switch (projectItem.Type)
				{
					case ProjectItemType.MILESTONE:
						//search database for a milestone
						table = "milestone";
						nameIndex = 1;
						descIndex = 2;
						startIndex = 3;
						endIndex = 4;
						break;
					case ProjectItemType.TASK:
						//search database for a task
						table = "task";
						nameIndex = 2;
						descIndex = 3;
						startIndex = 4;
						endIndex = 5;
						break;
					case ProjectItemType.SUBTASK:
						//search database for a subtask
						table = "subtask";
						nameIndex = 2;
						descIndex = 3;
						startIndex = 4;
						endIndex = 5;
						break;
				}

                conn.Open();
                string query = "select * from "+ table +"s where "+ table +"_id = '" + projectItem.id +"';";
                MessageBox.Show("Item selected: " + item.Header +"\nItem Type: " + projectItem.Type, Title);
                 
                MessageBox.Show(query);
                SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                //createCommand.ExecuteNonQuery();
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr.GetString(nameIndex);
                    string desc = dr.GetString(descIndex);
                    string start = dr.GetString(startIndex);
                    string end = dr.GetString(endIndex);

                    nameBox.Text = name;
                    descriptionBox.Text = desc;
                    startBox.Text = start;
                    endBox.Text = end;

                }
                
                

                //userNameBox.Items.Clear();
                //fill_userBox();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

                
       
     
    }
}
