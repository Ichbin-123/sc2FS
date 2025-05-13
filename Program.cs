class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        System.String? root = Directory.GetCurrentDirectory();
        DirectoryInfo? rootInfo = new DirectoryInfo(root);
        DirectoryInfo[]? dirsInfo = rootInfo.GetDirectories(); // var dirs = Directory.GetDirectories(root);
        Console.Clear();
        DirectoryInfo? documentiDir = CercaDocumentiFolder(rootInfo) ?? rootInfo;

        StampaDirectory(documentiDir, 0);   
        Console.WriteLine("\n");     
    } // Main
    static void PrintInformation(FileSystemInfo entita, int livelloIndentazione){      

        (ConsoleColor vorder, ConsoleColor hinter) colori;

        if(entita is DirectoryInfo directory){       // Cast a DirectoryInfo  
            colori = farbenZuruckgeben(directory);
            Console.ForegroundColor = colori.vorder;          
            for(int i=0; i<livelloIndentazione; i++){
                System.Console.Write("|   ");
            }
            Console.BackgroundColor = colori.hinter;      
            System.Console.WriteLine($"|--{directory.Name}");        
        } else if(entita is FileInfo file){
            colori = farbenZuruckgeben(file);
            Console.ForegroundColor = colori.vorder;  
            for(int i=0; i<livelloIndentazione; i++){
            System.Console.Write("|   ");
            }
            Console.BackgroundColor = colori.hinter;  
            System.Console.Write($"|--{file.Name}");        
            Console.Write(secMinHours(lastModify(file)));
            // Console.Write($" ({(int)lastModify(file).TotalMinutes} minuti fa)");
            Console.WriteLine("");
        }

        Console.ResetColor();
    } // PrintInformation
    static void StampaDirectory(DirectoryInfo directory, int livelloIndentazione){
        int counter = livelloIndentazione;
        DirectoryInfo[] directorisLocali = directory.GetDirectories();
        FileInfo[] filesLocali = directory.GetFiles();   
        
        counter++;
        foreach(var dir in directorisLocali){
            PrintInformation(dir, counter);
            StampaDirectory(dir, counter);
        }
        foreach(var fil in filesLocali){
            PrintInformation(fil, counter);
        }
    } // StampaDirectory
    static (ConsoleColor vordergrundFarbe, ConsoleColor hintergrundFarbe) farbenZuruckgeben(FileSystemInfo entita){
        ConsoleColor f1, f2;
        TimeSpan cinqueMinuti = new TimeSpan(0,5,0);
        TimeSpan trentaMinuti = new TimeSpan(0,30,0);

        if(entita is FileInfo file){
            if(lastModify(file)<cinqueMinuti){
                f1 = ConsoleColor.DarkMagenta;
                f2 = ConsoleColor.White;
            } else if (lastModify(file)>trentaMinuti){
                f1 = ConsoleColor.Gray;
                f2 = ConsoleColor.Black;
            } else {
            f1 = ConsoleColor.DarkYellow;
            f2 = ConsoleColor.Magenta;
            }
        } else {
            f1 = ConsoleColor.DarkGreen;
            f2 = ConsoleColor.Yellow;
        }
        return (vordergrundFarbe: f1, hintergrundFarbe: f2);
    } // farbenZuruckgeben
    static TimeSpan lastModify(FileInfo entita){
        DateTimeOffset adesso = DateTimeOffset.UtcNow;
        DateTimeOffset modify = entita.LastWriteTimeUtc;
        TimeSpan lastModify = adesso - modify;
        return lastModify;
    } // lastModify

    static string secMinHours(TimeSpan tempo){
        if(tempo <= new TimeSpan(0,0,59)){
           return $" ({(int)tempo.TotalSeconds} secondi fa)";
        } else if (tempo <= new TimeSpan(0,59,0)){
            return $" ({(int)tempo.TotalMinutes} minuti fa)";
        } else if (tempo <= new TimeSpan(23,0,0)){
            return $" ({(int)tempo.TotalHours} ore fa)";
        } else {
            return $" ({(int)tempo.TotalDays} giorni fa)";
        }
    }
    static DirectoryInfo? CercaDocumentiFolder(DirectoryInfo rootPassata){
        DirectoryInfo[]? localRoot = rootPassata.GetDirectories();
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        foreach (var dir in localRoot){
            if((string)dir.Name.ToString().ToLower()=="documenti"){
                System.Console.WriteLine($"|--{dir.Name}");
                Console.ResetColor();
                return dir;
            }
        }
        return null;
    } // CercaDocumentiFolder
} // Program