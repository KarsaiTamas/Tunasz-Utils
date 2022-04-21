# SaveLoadHandler
<br>Handles saving and loading from Json.</br>
<br>Default path location in SLHandler.cs is: Application.persistentDataPath + "\\" + "SaveFiles\\" you can change this if you want to handle your saves elsewhere.</br>
<br>Main reason I uploaded this to save it for my self, and made it public so other people can use it as well if they choose to.</br>
<br>This works with unity.</br>
<br>You can save and load any type with this.</br>
<br>To save with this you want to create a dedicated save class, where you put every data for each save file.</br>
<br>You can have multiple different save classes f.e. 1 for controls, 1 for gameplay etc. .</br>
<br>After you have your save class make a class variable in a script.</br>
<br>Call SLHandler.Save(yourClassVariable,"saveFileName"); To save the file</br>
<br>Call yourClassVariable=SLHandler.Load(defaultData,"saveFileName"); to load data</br>
<br>Make sure every class which you want to save is System.Serializable.</br>
<br>When you deleting save files ALWAYS make sure that you deleting the correct files.</br>
<br>The delete file is an in built funtion, if you delete something with it I won't take responsibility.</br>
