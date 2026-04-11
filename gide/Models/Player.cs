using System;
using System.Collections.Generic;

namespace gide.Models;

public partial class Player : ObservableObject
{
    private int _id;
    public int Id 
    { 
        get=>_id;
        set=>SetProperty(ref _id, value);
    }

    private string _username=null!;
    public string Username 
    { 
        get=>_username;
        set=>SetProperty(ref _username, value);
    }

    private string _password = null!;
    public string Password 
    { 
        get=>_password;
        set=>SetProperty(ref _password, value);
    }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
