using System;
using System.Collections.Generic;

namespace gide.Models;

public partial class Game : ObservableObject
{
    private int _id;
    public int Id 
    { 
        get=>_id;
        set=>SetProperty(ref _id, value);
    }

    private string _title = null!;
    public string Title 
    { 
        get=>_title;
        set=>SetProperty(ref _title, value); 
    }

    private string _description = null!;
    public string Description 
    { 
        get=>_description;
        set=>SetProperty(ref _description, value);
    }

    private string _fullProjectUrl = null!;
    public string FullProjectUrl 
    { 
        get=>_fullProjectUrl;
        set=>SetProperty(ref _fullProjectUrl, value);
    }

    private string _buildUrl = null!;
    public string BuildUrl 
    { 
        get=>_buildUrl; 
        set=>SetProperty(ref _buildUrl, value);
    }

    private string _nameExe = null!;
    public string NameExe
    {
        get => _nameExe;
        set => SetProperty(ref _nameExe, value);
    }

    private int _authorId;
    public int AuthorId 
    { 
        get=>_authorId;
        set=>SetProperty(ref _authorId, value);
    }
    private Author _author = null!;
    public virtual Author Author 
    { 
        get => _author;
        set => SetProperty(ref _author, value);
    }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
