   M S B A M L   `   `   `  ����[  WHelixToolkit.Wpf, Version=2012.3.77.1, Culture=neutral, PublicKeyToken=52aa3500039caf0dM8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.WpfHelixToolkit.Wpf  5   	    Trajectory Viewer"   Trajectory_Viewer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   QHelixToolkit8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.Wpf     5   	     � Title$  Trajectory Viewer��5   	   $	��600��6#   $	��600��60   .��5      ]� 
���   �.+�5   
   o� -   5        $
��menu��5   $   $	��Top=�6   $��22��60   .@�5      m� $
��File��6   .@�5	      m� -   5        $
��Open��5	      6)   m� 5
      -   5        $
��Save��5
      6)   m� 5      -   5        $��Clean Up Data��5      62   m� 5      -   5        $��	Kalmanize��5      6.   m� 5      -   5        $��	Write XML��5      6.   5      5      � 5       � RowDefinitions 5   
   �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6     � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��10 �6   .+�5      &    HelixToolkit.Wpf.HelixViewport3D  $��1��6+   $��0��68      ModelUpDirection$ 0,1,05�6I      CameraRotationMode$ 	Turntable=�6b      Camera 5      =� 5        � Position$ 0, 0, 51�6(    � LookDirection$ 0, 0, -15�6<    =� FieldOfView$ 45Q�6U   	 � UpDirection$	 0,1,05�6f   
   Children.
 5"         HelixToolkit.Wpf.SunLight  g� 5#      -   5        	model��5#       g� 5$      -   5        alltrajectories��5$       g� 5%      -	   5        singletrajectory��5%       5&      �� 5(      $��1��6   $��0��6&   $
��Left=�66   $	��Top=�6Q   $��60��6i   .+�5)      �� $��22��6   $��60��6%    �� Orientation$ 
Horizontal=�60   .+�5*      �� -
   5         ��Name$ tidtextblock��5*      $��40��60   
 � Tag$ Enter trajectory id��6;   6W   �� 5+      -   5        .��5+   8   Go6<   5,      �� 5-      -   5         ��Name$ showallbutton��5-      .��6H   
Hide All6R   5.      �� 50   
   $��1��6   $��0��6"   $��Bottom=�62   $
��Left=�6M   $	��100��6h   $	��120��6u    � 	Resources 51       �� 
TargetType   TextBlockStyle �� Property �� Value  
LabelStyle��%f  &	        &	 �     �� 52      " ��6   .�53      ��  � ��
 �����63   �� 54       � ��$ 10(�61   �� 55       � ��$ Bottom=�6:   56      �� 57      " ��6   .�58      ��  � ��
 �����63   �� 59       � ��$ 10(�61   �� 5:       � ��$
 Bold"�63   �� 5;       � ��$ Bottom=�6:   5<      5=      .+�5>      �� $ 
Horizontal=�6   .+�5?      �� #��[ 6   .��6<   Trajectory ID6K   �� 5@      -   5         ��Name$ tidblock��5@      #��[  6.   5A      �� 5B      $ 
Horizontal=�6   .+�5C      �� #��[ 6   .��6<   
Start Time6H   �� 5D      -   5        $ starttimeblock��5D      #��[  64   5E      �� 5F      $ 
Horizontal=�6   .+�5G      �� #��[ 6   .��6<   Length6D   �� 5H      -   5        $ lengthblock��5H      #��[  61   5I      �� 5J      $ 
Horizontal=�6   .+�5K      �� #��[ 6   .��6<   Average Velocity6N   �� 5L      -   5        $ averagevelocityblock��5L      #��[  6:   5M      5N      �� 5P   
   $��1��6   $��1��6$   $��Right=�64   $��Stretch=�5Q       �� ResizeBehavior$ PreviousAndNext=�63   $��5��6T   
�������6^   &   System.Windows.Controls.DataGrid  5S      -   5          Name$	 dg1��5S      $��1��6$   $��2��61      
IsReadOnly . 5T      5U      5V      5W      