   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MemoryGame.ViewModelsMemoryGame.ViewModels  5   	   3clr-namespace:MemoryGame.ViewsMemoryGame.Views  5   	        MemoryGame.Views.LoginView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :
viewmodels#clr-namespace:MemoryGame.ViewModels     5   	   0localclr-namespace:MemoryGame.Views     5   	     � Title$  Memory Game - Login��5	   	   $	��450��6%   $	��800��62   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6    $��Existing Users��60   $��0,0,10,0q�6H   .��5      ]� .+�5      �� $	��300��5      ��5      �� *Users+ �� SelectedItem 5      �� *SelectedUser+��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6)   .+�5   "   �� $��32��6L   $��32��6W   $��0,0,10,0q�6c   ��6(   �� *ProfileImagePath+6c   �� 5    "   $��Center=�6F   ��6,   �� *
Username+6F   5!      5"      5$      5%      5&      �� 5)   
   $��0��6   $��1��6    $��New User��60   $��10,0,0,0q�6B   .��5*      �� $��10q�6   .+�5+      �  5,      �� 5-      $
��Auto �6(   �� 5.      $
��Auto �6(    50      �� 51      $
��Auto �6+   �� 52      $��* �6+   �� 53      $
��Auto �6+   .+�56      �� $��	Username:��6    $��0��61   $��0��6>   $��0,0,10,0q�6N   �� 57      $��0��6`   $��1��6m   $��0,0,10,0q�6}   ��6   �� *NewUsername+ �� UpdateSourceTrigger$ PropertyChanged=�6}   �� 59      $��Profile Image:��6    $��1��66   $��0��6C   $��	0,10,10,0q�6S   �� 5:      $��1��6@   $��1��6M    �� 
IsReadOnly$
 True��6]   $��	0,10,10,0q�6o   ��6   �� *NewUserImagePath+6o   �� 5;      $��Browse��6   $��1��6.   $��2��6;   $��0,10,0,0q�6r   ��6K   �� *BrowseImageCommand+6r   5<      �� 5=      $��Add User��6   $��0,10,0,0q�6O   ��6,   �� *AddUserCommand+6O   5>      5?      �� 5B   
   $��2��6   $��0��6"   $��2��62   $ 
Horizontal=�6F   $��Right=�6_   $��0,10,0,0q�6{   .+�5C      �� $��Delete User��6   $��0,0,10,0q�6Q   ��6+   �� *DeleteUserCommand+6Q   �� 5D      $
��Play��6   ��6$   �� *PlayCommand+5E      5F      5G      