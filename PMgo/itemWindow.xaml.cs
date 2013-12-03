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
            fillUserBox();
            this.ProjectNameValue = this.projNameBox.Text;
            PopulateTreeView();
            fillDocBox();
            fillAssignedUsersBox();

			//need to populate the treeview before expaninding it.
            expandTreeView(ItemTreeView);
			this.completeButton.Visibility = Visibility.Hidden;
            this.notCompleteButton.Visibility = Visibility.Hidden;
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
                fillProjectManagerBox();
                expandTreeView(ItemTreeView);
            }
            
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
                 string query = "select user_name from users join projects on (projects.proj_mgr = users.id) where projects.project_name = '" + this.projNameBox.Text + "' and users.id = projects.proj_mgr;";
                 SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                 SQLiteDataReader dr = createCommand.ExecuteReader();

                 while (dr.Read())
                 {
                     string manager = dr.GetString(0);
                     //MessageBox.Show(manager);
                     //MessageBox.Show(this.current_txt.Text);
                     if (manager != this.current_txt.Text)
                     {
                         deleteButton.Visibility = Visibility.Hidden;
                         addButton.Visibility = Visibility.Hidden;
                         removeButton.Visibility = Visibility.Hidden;
                         
                     }
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }

         }

         void fillDocBox()
         {
             SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

             // open connection to database
             try
             {
                 conn.Open();
                 if (this.typeBox.Text == "task")
                 {
                     //MessageBox.Show(this.project_txt.Text);
                     string Query = "select doc_name from documents where task_id = (select task_id from tasks where task_name = '" + this.nameBox.Text + "');";
                     SQLiteCommand createcommand = new SQLiteCommand(Query, conn);                    
                     SQLiteDataReader dr = createcommand.ExecuteReader();

                     while (dr.Read())
                     {
                         string doc = dr.GetString(0);
                         docBox.Items.Add(doc);
                     }
                     
                 }
                 else if (this.typeBox.Text == "milestone")
                 {
                     //MessageBox.Show(this.project_txt.Text);
                     string Query = "select doc_name from documents where milestone_id = (select milestone_id from milestones where milestone_name = '" + this.nameBox.Text + "');";
                     SQLiteCommand createcommand = new SQLiteCommand(Query, conn);
                     SQLiteDataReader dr = createcommand.ExecuteReader();

                     while (dr.Read())
                     {
                         string doc = dr.GetString(0);
                         docBox.Items.Add(doc);
                     }

                 }
                 if (this.typeBox.Text == "subtask")
                 {
                     //MessageBox.Show(this.project_txt.Text);
                     string Query = "select doc_name from documents where subtask_id = (select subtask_id from subtasks where subtask_name = '" + this.nameBox.Text + "');";
                     SQLiteCommand createcommand = new SQLiteCommand(Query, conn);                     
                     SQLiteDataReader dr = createcommand.ExecuteReader();

                     while (dr.Read())
                     {
                         string doc = dr.GetString(0);
                         docBox.Items.Add(doc);
                     }

                 }
                 
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }

         void fillAssignedUsersBox()
         {
             SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
             assignedUsersBox.Items.Clear();
             // open connection to database
             try
             {
                 
                 conn.Open();
                 if (this.typeBox.Text == "task")
                 {
                     //MessageBox.Show(this.project_txt.Text);
                     string query = "select user_name from users join users_tasks on (users_tasks.user_id = users.id) where users.id = users_tasks.user_id and users_tasks.task_id = (select task_id from tasks where task_name = '" + nameBox.Text + "');";
                     //MessageBox.Show(query);
                     SQLiteCommand createcommand = new SQLiteCommand(query, conn);
                     SQLiteDataReader dr = createcommand.ExecuteReader();

                     while (dr.Read())
                     {
                         string userName = dr.GetString(0);
                         assignedUsersBox.Items.Add(userName);
                     }

                 }
                 else if (this.typeBox.Text == "subtask")
                 {
                     //MessageBox.Show(this.project_txt.Text);
                     string query = "select user_name from users join users_subtasks on (users_subtasks.user_id = users.id) where users.id = users_subtasks.user_id and users_subtasks.subtask_id = (select subtask_id from subtasks where subtask_name = '" + nameBox.Text + "');";
                     //MessageBox.Show(query);
                     SQLiteCommand createcommand = new SQLiteCommand(query, conn);
                     SQLiteDataReader dr = createcommand.ExecuteReader();

                     while (dr.Read())
                     {
                         string userName = dr.GetString(0);
                         assignedUsersBox.Items.Add(userName);
                     }

                 }
                 
                 
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
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
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                if (this.typeBox.Text == "task")
                {
                    string query = "insert into users_tasks (user_id, task_id) select id, task_id from users, tasks where users.user_name = '"
                                + this.availableUsersBox.SelectedItem + "'and tasks.task_name = '" + this.nameBox.Text + "';";
                    //MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("User was Assigned!");
                    fillAssignedUsersBox();
                    conn.Close();
                }
                else if (this.typeBox.Text == "subtask")
                {
                    string query = "insert into users_subtasks (user_id, subtask_id) select id, subtask_id from users, subtasks where users.user_name = '"
                                + this.availableUsersBox.SelectedItem + "'and subtasks.subtask_name = '" + this.nameBox.Text + "';";
                    //MessageBox.Show(query);
                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    MessageBox.Show("User was Assigned!");
                    fillAssignedUsersBox();
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("You must Choose a Task or a Subtask to add a Team Member to!");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("User is already assigned!");
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CreateComboBox_Initialized(object sender, EventArgs e)
        {
            CreateComboBox.SelectedValue = CreateComboBox.Items[0];
        }

        public void PopulateTreeView()
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

        void fillProjectManagerBox()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();
                string query = "select user_name from users join projects on (projects.proj_mgr = users.id) where project_name = '" + this.projNameBox.Text + "';";
                //MessageBox.Show(query);
                SQLiteCommand createcommand = new SQLiteCommand(query, conn);
                SQLiteDataReader dr = createcommand.ExecuteReader();
                while (dr.Read())
                {
                    this.projectManagerBox.Text = dr.GetString(0);                    
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void fillUserBox()
        {
             SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionString);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
              string Query = "select user_name from users;";
           
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                //MessageBox.Show(Query);
                // createcommand.ExecuteNonQuery();
                SQLiteDataReader dr = createcommand.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    availableUsersBox.Items.Add(name);
                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        void statusButtons()
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            // open connection to database
            conn.Open();
            try
            {

                string Query = "";
                if (this.typeBox.Text == "task")
                {
                    Query = "select complete from tasks where task_id = (select task_id where task_name = '" + this.nameBox.Text + "');";
                }
                else if (this.typeBox.Text == "milestone")
                {
                    Query = "select complete from milestones where milestone_id = (select milestone_id where milestone_name = '" + this.nameBox.Text + "');";
                }
                else if (this.typeBox.Text == "subtask")
                {
                    Query = "select complete from subtasks where subtask_id = (select subtask_id where subtask_name = '" + this.nameBox.Text + "');";
                }              
                SQLiteCommand createCommand = new SQLiteCommand(Query, conn);
                SQLiteDataReader dr = createCommand.ExecuteReader();
                while (dr.Read())
                {
                    int complete = dr.GetInt32(0);
                    if (complete == 0)
                    {
                        this.completeButton.Visibility = Visibility.Hidden;
                        this.notCompleteButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.completeButton.Visibility = Visibility.Visible;
                        this.notCompleteButton.Visibility = Visibility.Hidden;
                    }
                }
                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //MessageBox.Show("Item selected: " + item.Header +"\nItem Type: " + projectItem.Type, Title);
                 
                //MessageBox.Show(query);
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
                    typeBox.Text = table;

                    if (typeBox.Text == "milestone")
                    {
                        TypeFieldBox.Text = "Milestone";
                    }

                    else if (typeBox.Text == "task")
                    {
                        TypeFieldBox.Text = "Task";
                    }

                    else if (typeBox.Text == "subtask")
                    {
                        TypeFieldBox.Text = "Subtask";
                    }
                    statusButtons();
                }              
                

                docBox.Items.Clear();
                fillDocBox();
                fillAssignedUsersBox();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            MessageBoxResult result = MessageBox.Show("Are you sure you want to mark complete?", "Change Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
            try
            {
                conn.Open();
                string Query = "";
                if (this.typeBox.Text == "task")
                {
                    Query = "update tasks set complete = 1 where task_id = (select task_id where task_name = '" + this.nameBox.Text + "');";
                }
                else if (this.typeBox.Text == "milestone")
                {
                    Query = "update milestones set complete = 1 where milestone_id = (select milestone_id where milestone_name = '" + this.nameBox.Text + "');";
                }
                else if (this.typeBox.Text == "subtask")
                {
                    Query = "update subtasks set complete = 1 where subtask_id = (select subtask_id where subtask_name = '" + this.nameBox.Text + "');";
                }                

                SQLiteCommand createcommand = new SQLiteCommand(Query, conn);
                createcommand.ExecuteNonQuery();
                MessageBox.Show("Item is now complete!");
                this.notCompleteButton.Visibility = Visibility.Hidden;
                this.completeButton.Visibility = Visibility.Visible;
                ItemTreeView.Items.Clear();
                PopulateTreeView();
				
                expandTreeView(ItemTreeView);
                

                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
            
        }

        private void submit_btn_Copy1_Click(object sender, RoutedEventArgs e)
        {
            if (CreateComboBox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Milestone")
            {
                String projectName = this.projNameBox.Text;
                createMilestone mile = new createMilestone();
                mile.ProjectValue = projectName;
                mile.ShowDialog();

				//no need to close the window, just refresh it after closing the create window
				PopulateTreeView();
				expandTreeView(ItemTreeView);
			}
            if (CreateComboBox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Task")
            {
                String projectName = this.projNameBox.Text;
                createItem item = new createItem();
                item.ProjectValue = projectName;
                item.ShowDialog();
                //after the createItem window closes:
                PopulateTreeView();
				expandTreeView(ItemTreeView);
            
            }

            else if (CreateComboBox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Subtask")
            {
                if (this.nameBox.Text == "")
                {
                    MessageBox.Show("You must first choose a task!");
                }
                else if (this.typeBox.Text != "task")
                {
                    MessageBox.Show("This is not a task.  Please Choose a task to add a subtask to.");
                }
                else
                {
                    String projectName = this.projNameBox.Text;
                    String taskName = this.nameBox.Text;
                    createSubtask subtask = new createSubtask();
                    subtask.ProjectValue = projectName;
                    subtask.TaskValue = taskName;
                    subtask.ShowDialog();

					PopulateTreeView();
					expandTreeView(ItemTreeView);
				}
                
            }

            else if (CreateComboBox.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Document")
            {
                if (this.nameBox.Text == "")
                {
                    MessageBox.Show("You must first choose a task or a subtask!");
                }               
                else
                {
                    String type = this.typeBox.Text;
                    String parent = this.nameBox.Text;
                    createDoc doc = new createDoc();
                    doc.TypeValue = type;
                    doc.ParentValue = parent;
                    doc.ShowDialog();
                    
                }

            }
            else
            {
                MessageBox.Show("You must first choose a 'Project Item' to create");
            }
            
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);
            try
            {

                if (this.typeBox.Text == "task")
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to Delete this Task?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        conn.Open();
                        string query = "delete from tasks where task_name = '" + this.nameBox.Text + "' and proj_id = (select project_id from projects where project_name = '" + this.projNameBox.Text + "')";

                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        MessageBox.Show("Item has been deleted!");
                        conn.Close();
                        this.Close();
                        string projectName = this.projNameBox.Text;
                        string username = this.current_txt.Text;
                        itemWindow update = new itemWindow();
                        update.ProjectNameValue = projectName;
                        update.UserValue = username;
                        update.ShowDialog();

                        this.Close();
                    }
                    
                    
                }
                else if (this.typeBox.Text == "subtask")
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to Delete this subtask?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        conn.Open();
                        string query = "delete from subtasks where subtask_name = '" + this.nameBox.Text + "';";

                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        MessageBox.Show("Item has been deleted!");
                        conn.Close();
                        this.Close();
                        string projectName = this.projNameBox.Text;
                        string username = this.current_txt.Text;
                        itemWindow update = new itemWindow();
                        update.ProjectNameValue = projectName;
                        update.UserValue = username;
                        update.ShowDialog();

                        this.Close();
                    }
                }
                else if (this.typeBox.Text == "milestone")
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to Delete this milestone?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        conn.Open();
                        string query = "delete from milestones where milestone_name = '" + this.nameBox.Text + "';";

                        SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                        createCommand.ExecuteNonQuery();
                        MessageBox.Show("Milestone has been deleted!");
                        conn.Close();
                        this.Close();
                        string projectName = this.projNameBox.Text;
                        string username = this.current_txt.Text;
                        itemWindow update = new itemWindow();
                        update.ProjectNameValue = projectName;
                        update.UserValue = username;
                        update.ShowDialog();

                        this.Close();
                    }
                }
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void docBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String name = this.docBox.SelectedItem.ToString();
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionString);

            // open connection to database
            try
            {
                sqliteCon.Open();

                //MessageBox.Show(this.project_txt.Text);
                string Query = "select * from documents where doc_name = '" + name + "';";
                SQLiteCommand createcommand = new SQLiteCommand(Query, sqliteCon);
                SQLiteDataReader dr = createcommand.ExecuteReader();
                while (dr.Read())
                {
                    string dName = dr.GetString(1);
                    string dDesc = dr.GetString(2);
                    string dLocation = dr.GetString(3);

                    viewDoc viewDocument = new viewDoc();
                    viewDocument.NameValue = dName;
                    viewDocument.DescValue = dDesc;
                    viewDocument.LocationValue = dLocation;
                    viewDocument.ShowDialog();
                    

                }
                sqliteCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void availableUsersBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection(dbConnectionString);

            try
            {
                conn.Open();

                if (this.typeBox.Text == "task")
                {
                    string query = "update tasks set task_name = '" + this.nameBox.Text
                            + "', task_description = '" + this.descriptionBox.Text
                            + "', task_start = '" + this.startBox.Text
                            + "', task_end = '" + this.endBox.Text
                            + "'";

                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();
                    
                    MessageBox.Show("Task Was Modified!");
                    conn.Close();
                    
                }
                else if (this.typeBox.Text == "subtask")
                {
                    string query = "update subtasks set subtask_name = '" + this.nameBox.Text
                            + "', subtask_description = '" + this.descriptionBox.Text
                            + "', subtask_start = '" + this.startBox.Text
                            + "', subtask_end = '" + this.endBox.Text
                            + "'";

                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();

                    MessageBox.Show("Subtask Was Modified!");
                    conn.Close();
                }
                else if (this.typeBox.Text == "milestone")
                {
                    string query = "update milestones set milestone_name = '" + this.nameBox.Text
                            + "', milestone_desc = '" + this.descriptionBox.Text
                            + "', milestone_start = '" + this.startBox.Text
                            + "', milestone_end = '" + this.endBox.Text
                            + "' where milestone_name = '" + this.nameBox.Text + "'";

                    SQLiteCommand createCommand = new SQLiteCommand(query, conn);
                    createCommand.ExecuteNonQuery();

                    MessageBox.Show("Milestone Was Modified!");
                    conn.Close();
                }
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

                
       
     
    }
}
