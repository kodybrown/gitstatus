gitstatus
=========

Displays an interactive git status on the command-line.

This is a simple application that allows you to get the status of your Git directory interactively. What I mean by interactively is you can diff files* and mark files for adding right from the command-line. 

*In order to diff files you must create a Git alias named 'difftool' that opens/launches an external application to perform the diff. If the alias does not open a different tool (something other than the built in 'diff' command, it will be ignored and potentially lock-up the gitstatus application.

Currently, it only works on modified files. It won't let you add files that are not already part of the repository. I only felt that this utility was needed when modifying files, etc. Let me know, if you would like it extended or fork it yourself! 

Yes, it is very quick and dirty. However, it works nicely for what it was made for!

gitstatus application and source code is released using the MIT License, see LICENSE.txt for details.

You can download the compiled executable from my dropbox here:
* [gitstatus(.net2.0)][1]
* [gitstatus(.net3.0)][2]
* [gitstatus(.net3.5)][3]
* [gitstatus(.net4.0)][4]
* [gitstatus(.net4.5)][5]

[1]: http://kodybrown.com/url/1J        "gitstatus(.net2.0)"
[2]: http://kodybrown.com/url/1K        "gitstatus(.net3.0)"
[3]: http://kodybrown.com/url/1L        "gitstatus(.net3.5)"
[4]: http://kodybrown.com/url/1M        "gitstatus(.net4.0)"
[5]: http://kodybrown.com/url/1N        "gitstatus(.net4.5)"
