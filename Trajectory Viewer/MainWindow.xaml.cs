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

        //private void Reset_Click(object sender, RoutedEventArgs e)
        //{
        //    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, 5);
        //    model.Transform = new Transform3DGroup();
        //}

        //private void ShowAllButton_Clicked(object sender, RoutedEventArgs e)
        //{
            

        //}

        //private void Grid_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (mDown)
        //    {
        //        Point pos = Mouse.GetPosition(viewport);
        //        Point actualPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
        //        double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

        //        double mouseAngle = 0;
        //        if (dx != 0 && dy != 0)
        //        {
        //            mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
        //            if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
        //            else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
        //            else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
        //        }
        //        else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
        //        else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

        //        double axisAngle = mouseAngle + Math.PI / 2;

        //        Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

        //        double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

        //        Transform3DGroup group = model.Transform as Transform3DGroup;
        //        QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
        //        group.Children.Add(new RotateTransform3D(r));

        //        mLastPos = actualPos;
        //    }
        //}

        //private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    mDown = false;
        //}

        //private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton != MouseButtonState.Pressed) return;
        //    mDown = true;
        //    Point pos = Mouse.GetPosition(viewport);
        //    mLastPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
        //}

        //private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 250D);
        //}

        private void mnuOpen_Clicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            OpenFileDialog1.Title = "Open Trajectory Data";
            OpenFileDialog1.DefaultExt = ".xml";
            OpenFileDialog1.Filter = "XML files (*.xml)|*.txt|All files (*.*)|*.*";

            Nullable<bool> result = OpenFileDialog1.ShowDialog();

            if (result == true)
            {
                loadData(OpenFileDialog1.FileName);

                model.Children.Clear();
                model.Children.Add(axes);

                showAll();
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

        private void ShowTrajButton_Clicked(object sender, RoutedEventArgs e)
        {
            int tid = 0;
            if (tds != null)
            {
                singletrajectory.Children.Clear();

                if (int.TryParse(tidtextblock.Text, out tid) && tds.trajectories.FindByt_id(tid) != null)
                {
                    DrawTrajectory(tid, Colors.Red, "N", singletrajectory);
                }
            }                
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
    }
}

