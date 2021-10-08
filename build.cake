//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var PostName = Argument("PostName", "");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Test")
    .Does(() =>
{
    // hugo isnt shutdown, so kill it if its running
    StartProcess("taskkill", "-f /im hugo.exe"); 

    var hugoProcessSettings = new ProcessSettings
    { 
        Arguments = "server -D",
        RedirectStandardOutput = true,
        RedirectStandardError= true
    };

    // start hugo with drafts on a specific port
    using(var process = StartAndReturnProcess("hugo", hugoProcessSettings))
    {
        // process.WaitForExit();
        // Information("{0}", process);
    }

    // start chrome for viewing
    using(var process = StartAndReturnProcess(@"c:\program files (x86)\google\chrome\application\chrome.exe", new ProcessSettings{ Arguments = "http://localhost:1313/CookingRecipies/" }))
    {
        // process.WaitForExit();
        // Information("{0}", process);
    }
});

Task("NewPost")
    .Does(() =>
{
    // setup the new posts name
    var newPostName = string.Format("{0}", PostName);

    Information("NewPostName \"{0}\"", newPostName);
    if(string.IsNullOrWhiteSpace(newPostName))
    {
        throw new Exception("Please specify a name for the post");
    }

    if (!newPostName.EndsWith(".md")) 
    {
        newPostName += ".md";
    }

    var fullPostName = string.Format("posts/{0}", newPostName);

    var checkForFilePostName = string.Format("content/{0}", fullPostName);
    if (FileExists(checkForFilePostName))
    {
        throw new Exception(string.Format("The file \"{0}\" already exists!", checkForFilePostName));
    }
    
    var hugoProcessSettings = new ProcessSettings
    { 
        Arguments = string.Format("new {0}", fullPostName),
        RedirectStandardOutput = false,
        RedirectStandardError= false
    };

    // create new posts file with hugo
    using(var process = StartAndReturnProcess("hugo", hugoProcessSettings))
    {
        // process.WaitForExit();
        // Information("{0}", process);
    };

    // start visual studio code for editing
    using(var process = StartAndReturnProcess(@"C:\Program Files\Microsoft VS Code\Code.exe", new ProcessSettings{ Arguments = checkForFilePostName }))
    {
        // process.WaitForExit();
        // Information("{0}", process);
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
