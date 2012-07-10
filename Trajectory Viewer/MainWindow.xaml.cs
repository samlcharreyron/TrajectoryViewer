using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Petzold.Media3D;
using System.Windows.Media.Media3D;
using System.IO;
using System.Data;
using HelixToolkit.Wpf;


namespace Trajectory_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeometryModel3D mGeometry;

        private bool mDown;
        private Point mLastPos;
        private TrajectoryDbDataSet tds;
        private Axes axes;

        private bool allshown;

        //private ModelVisual3D model;
        Petzold.Media2D.ArrowEnds arrow;

        public MainWindow()
        {
            InitializeComponent();

            //model = new ModelVisual3D();
            model.Transform = new Transform3DGroup();

            axes = new Axes()
            {
                Color = Colors.White
            };

            model.Children.Add(axes);

        }

        private void loadData(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                try
                {
                    DataSet ds = new DataSet();

                    ds.ReadXml(fs);

                    tds = new TrajectoryDbDataSet();
                    tds.Merge(ds);

                    //updateDatabase();
                    foreach (TrajectoryDbDataSet.trajectoriesRow row in tds.trajectories)
                    {
                        //Console.WriteLine(row.average_velocity.CompareTo(Double.NaN));
                        if (row.average_velocity.CompareTo(Double.NaN) == 0)
                        {
                            row.average_velocity = 0.0;
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        private void mnuOpen_Clicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            OpenFileDialog1.Title = "Open Trajectory Data";
            OpenFileDialog1.DefaultExt = "*.xml";
            OpenFileDialog1.Filter = "XML files (*.xml)|*.txt|All files (*.*)|*.*";

            Nullable<bool> result = OpenFileDialog1.ShowDialog();

            if (result == true)
            {
                loadData(OpenFileDialog1.FileName);

                model.Children.Clear();
                model.Children.Add(axes);

                showAll();
                //cleanUpData();
                addToDataGrid();
            }
        }

        private void showAll()
        {
            for (int i = 0; i < tds.trajectories.Count; i++)
            {
                DrawTrajectory(tds.trajectories[i].t_id, Colors.Green,tds.trajectories[i].average_direction,alltrajectories);
            }

            showallbutton.Content = "Hide All";
            allshown = true;
        }

        private void hideAll()
        {
            try
            {
                alltrajectories.Children.Clear();
                showallbutton.Content = "Show All";
                allshown = false;
            }
            catch
            {
                return;
            }
        }

        private void DrawTrajectory(int t_id, Color color,string direction,ModelVisual3D model)
        {

            //find trajectory id of last inserted trajectory

            if (tds.trajectories.Count == 0)
            {
                return;
            }

            //int t_id = Globals.ds.trajectories[Globals.ds.trajectories.Count - 1].t_id;

            TrajectoryDbDataSet.pointsRow[] pointsRows = (TrajectoryDbDataSet.pointsRow[])tds.points.Select("t_id = " + t_id);

            if (pointsRows.Count() < 30 && pointsRows.Count() > 2)
            {
                Point3D firstPoint = new Point3D((float)pointsRows[0].X, (float)pointsRows[0].Y, (float)pointsRows[0].Z);

                Point3DCollection pointCollection = new Point3DCollection();

                for (int i = 1; i < pointsRows.Length; i++)
                {
                    TrajectoryDbDataSet.pointsRow currentRow;
                    TrajectoryDbDataSet.pointsRow lastRow;

                    try
                    {
                        currentRow = pointsRows[i];
                        lastRow = pointsRows[i - 1];
                    }
                    catch (Exception e)
                    {
                        return;
                    }

                    pointCollection.Add(new Point3D(currentRow.X, currentRow.Y, currentRow.Z));

                }


                if (direction.Equals("R"))
                {
                    arrow = Petzold.Media2D.ArrowEnds.End;
                }
                else if (direction.Equals("L"))
                {
                    arrow = Petzold.Media2D.ArrowEnds.Start;
                }
                else arrow = Petzold.Media2D.ArrowEnds.None;

                WirePolyline wl = new WirePolyline()
                {
                    Points = pointCollection,
                    Thickness = 1,
                    Rounding = 1,
                    Color = color,
                    ArrowEnds = Petzold.Media2D.ArrowEnds.End
                };

                model.Children.Add(wl);
                model.Transform = new Transform3DGroup();
            }
        }

        private void updateTrajectoryText(int tid)
        {
            TrajectoryDbDataSet.trajectoriesRow traj = tds.trajectories.FindByt_id(tid);
            tidblock.Text = traj.t_id.ToString();
            starttimeblock.Text = traj.start_time.ToString("HH:mm:ss.fff");
            lengthblock.Text = traj.length.ToString("0.000");
            averagevelocityblock.Text = traj.average_velocity.ToString("0.000");
            
        }

        private void clearTrajectoryText()
        {
            tidblock.Text = "";
            starttimeblock.Text = "";
            lengthblock.Text = "";
            averagevelocityblock.Text = "";
        }


        //Used to recalculate velocities based on stored millisecond values.  Discards the first point
        private void cleanUpData()
        {
            TrajectoryDbDataSet.pointsRow lastRow = null;

            for (int i = 0; i < tds.trajectories.Count; i++)
            {
                if (tds.trajectories[i].average_velocity > 0)
                {
                    foreach (TrajectoryDbDataSet.pointsRow row in tds.points.Select(String.Format("t_id = {0}", tds.trajectories[i].t_id)))
                    {
                        if (lastRow != null)
                        {
                            row.deltaDistance = Math.Sqrt(Math.Pow((row.X - lastRow.X), 2) + Math.Pow(row.Z - lastRow.Z, 2));
                            row.distance = lastRow.distance + row.deltaDistance;
                            row.velocity = 1000 * row.deltaDistance / (row.milliseconds - lastRow.milliseconds);
                        }
                        else row.distance = 0;

                        lastRow = row;
                    }

                    tds.trajectories[i].average_velocity = 1000 * lastRow.distance / lastRow.milliseconds;
                    lastRow = null;
                }
            }
        }

        private void addToDataGrid()
        {
            BindingListCollectionView view = CollectionViewSource.GetDefaultView(tds.trajectories) as BindingListCollectionView;
            dg1.ItemsSource = view;
        }

        private void ShowTrajButton_Clicked(object sender, RoutedEventArgs e)
        {
            int tid = 0;
            if (tds != null)
            {
                singletrajectory.Children.Clear();
                clearTrajectoryText();
    

                if (int.TryParse(tidtextblock.Text, out tid) && tds.trajectories.FindByt_id(tid) != null)
                {
                    DrawTrajectory(tid, Colors.Red, "N", singletrajectory);
                    updateTrajectoryText(tid);
                    //highlightRow(tid);
                }
            }                
        }

        private void highlightRow(int tid)
        {
            //throw new NotImplementedException();
            //DataGridRow row = (DataGridRow)dg1.ItemContainerGenerator.ContainerFromIndex(tid);
            try
            {
                throw new NotImplementedException("not yet implemented");
                //TrajectoryDbDataSet.trajectoriesRow trajrow = tds.trajectories.FindByt_id(tid);
                //DataGridRow row = dg1.ItemContainerGenerator.ContainerFromItem(trajrow) as DataGridRow;
                //row.Background = new SolidColorBrush(Colors.Yellow);
                //dg1.ScrollIntoView(row);
            }
            catch
            {
                return;
            }

            //throw new ArgumentException("No item with specified id exists in the dataGrid.", "id");
            

        }

        private void showallbutton_Click(object sender, RoutedEventArgs e)
        {
            if (tds != null)
            {
                if (allshown)
                {
                    hideAll();
                }
                else showAll();
            }
        }

        private void mnuSave_Clicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Title = "Save Trajectory Data";
            saveFileDialog1.DefaultExt = ".csv";
            saveFileDialog1.Filter = "CSV files (*.csv)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FileName = "trajectories.csv";

            Nullable<bool> result = saveFileDialog1.ShowDialog();

            if (result == true)
            {
                string saveFileName = saveFileDialog1.FileName;

                try
                {
                    // Create the CSV file to which grid data will be exported.

                    StreamWriter sw = new StreamWriter(saveFileName, false);
                    // First we will write the headers.
                    DataTable dt = tds.trajectories;
                    int iColCount = dt.Columns.Count;
                    for (int i = 0; i < iColCount; i++)
                    {
                        sw.Write(dt.Columns[i]);
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);

                    // Now write all the rows.

                    //foreach (DataRow dr in dt.Rows)
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr = dt.Rows[j];

                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                sw.Write(dr[i].ToString());
                            }
                            if (i < iColCount - 1)
                            {
                                sw.Write(",");
                            }
                        }

                        sw.Write(sw.NewLine);
                    }
                    sw.Close();

                    MessageBox.Show("Trajectories succesfully saved to CSV file");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void mnuCleanUp_Clicked(object sender, RoutedEventArgs e)
        {
            cleanUpData();
        }
    }
}

