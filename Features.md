
```
1. Move type to new file in different project.
2. Create class in a referenced project.
3. Create interface in a referenced project.
4. Plug in auto update.
```
### Details ###

1. Move type to new file in different project.
  * When cursor is on a class/interface name and is the second(or greater) type declared in a code file.  A new set of context actions(Yellow light bulb) options will appear.
  * A new menu option will be created for each project that is referenced by the current project.  i.e. if you are in a UnitTest project the projects that are already referenced by this project will show up.
> Here is an example of the context action working in the MvcContrib source code. The items circled in red were added by this plugin.
http://resharper-tdd-productivity-plugin.googlecode.com/svn/content/MoveToProjectScreenShot.JPG

2. Create class in a referenced project.
tbd

3. Create interface in a referenced project.
tbd

4. Auto update.
> On startup the plugin will make a single web request to a text file in the projects source code repository.  The file that it calls is http://resharper-tdd-productivity-plugin.googlecode.com/svn/trunk/LatestVersion/version.txt  This call does not collect any information about the user of the plugin and is not tracked in anyway.