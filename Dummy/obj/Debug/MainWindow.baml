   M S B A M L   `   `   `  ����@  <Dummy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DummyDummy  5   	   F BMediaPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:MediaPlayer;assembly=MediaPlayerMediaPlayer 5   	   C ?Playlist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5(clr-namespace:Playlist;assembly=PlaylistPlaylist 6L       Dummy.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Dummy     5   	   FMediaPlayer.clr-namespace:MediaPlayer;assembly=MediaPlayer     5   	   =Playlist(clr-namespace:Playlist;assembly=Playlist     6L   -   5          � Title$  XPlayer��5	   	   $��536.975��6   $��1013.158��6*    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ MediaCommands.Play��5      5        5   
   -   5        $ MediaCommands.Stop��5      5   .    � 	Resources 5         	PlayImage H� Drawing � Brush � Geometry  	StopImage��%/  &	        &	 �     H� 5   
    5      I� 5      ��5      � 
 � � �+ �����@x}�        �    ���р��@x}68   5      5      H� 5   
    5      I� 5      ��5       � 
 �����4 ��        р��    р������    ����        67   5"      5$      5%       � TaskbarItemInfo 5'      *  $System.Windows.Shell.TaskbarItemInfo  5)      -   5        taskBarItemInfo1��   Overlay# [  5*      	   ThumbnailClipMargin$	 80,0,80,140q�5+      
   Description$
 Taskbar Item Info Sample��5,         ThumbButtonInfos 5.      4  .System.Windows.Shell.ThumbButtonInfoCollection  *  $System.Windows.Shell.ThumbButtonInfo  5/      -   5           DismissWhenClicked .     Command$ MediaCommands.Play��52         Description$
 Play��54         ImageSource# [  55         CommandTarget 53      ��  �� ElementName$ btnPlay��55        56      -   5         . $ MediaCommands.Stop��59      $
 Stop��5;      # [ 5<       5:      �� $ btnStop��5<      5=      5?      .��5A      � -   5         �Name$ 	Container��5A       � ColumnDefinitions 5B   
   �� 5C      �� 5D      $��0* �6   .+�5F   
   o� -   5         o�Name$ toolMenu��5F      $
��Left=�6!   $��20��6<   $	��Top=�6H   $
��1005��6`   .@�5G      m� $��_File��6   .@�5H      m� -	   5        $��
_Open File��5H      6/   m� 5I      -
   5        $��_Exit��5I      6*   5J      5K        MediaPlayer.UserControl1  5L   
   -   5          Name$ MediaPlayer��5L   #   $
��Left=�69   $	��484��6T   $��0,20,0,0q�6a   $	��Top=�6s   $	��753��6�     Playlist.UserControl1  5M   
   -   5          Name$ playList��5M       $
��Left=�63   $	��484��6N   $��
753,20,0,0q�6[   $	��Top=�6o   $	��252��6�   5N      5O      