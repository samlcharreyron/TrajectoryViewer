   M S B A M L   `   `   `  ����L  HTrajectory Viewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Trajectory_ViewerTrajectory_Viewer  5   	   $    Trajectory_Viewer.PointsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   .myclr-namespace:Trajectory_Viewer     5   	     � Title$  PointsWindow��5   	   $	��600��6   $	��600��6+    � 	Resources 5      +   %Trajectory_Viewer.TrajectoryDbDataSet   trajectoryDbDataSet  pointsViewSource �� Source �� Path �� Source��%Y   &	        &	      7     5   
   6!   �� 5   
    68   �� $ points�8   5	      .��5
      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ pointsDg��5         AutoGenerateColumns .     EnableRowVirtualization . $	��548��6`      
IsReadOnly . ��6m   �� # [ 	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        xColumn��,  &System.Windows.Controls.DataGridColumn
   Header$
 X��5   Q      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6\   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   �� $ X�6\     5      -   5        yColumn��$
 Y��5   Q   $ 60 6\    66   �� $ Y�6\     5      -   5        zColumn��$
 Z��5   Q   $ 60 6\    66   �� $ Z�6\     5      -   5        velocityColumn��$
 Velocity��5   _   $ 60 6q    6=   �� $ velocity�6q     5      -   5        vxColumn��$
 vx��5   S   $ 60 6_    67   �� $ vx�6_     5      -   5        vzColumn��$
 vz��5   S   $ 60 6_    67   �� $ vz�6_     5      -   5        msColumn��$
 milliseconds��5   ]   $ SizeToHeader 6s    67   �� $ milliseconds�6s     5      -	   5        	tidColumn��$

 t_id��5   V   $ 30 6d    68   �� $
 t_id�6d     5      -
   5        directionColumn��$
 	direction��5   a   $ SizeToHeader 6t    6>   �� $ 	direction�6t   5      5      5      