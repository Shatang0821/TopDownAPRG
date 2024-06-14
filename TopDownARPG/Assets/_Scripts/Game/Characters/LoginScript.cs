using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class User
{
    public string username;
    public string password;
}

public class LoginScript : MonoBehaviour

{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField confirmPasswordInput;
    public Text registrationMessageText;

    private User savedUser;

    private void Start()
    {
        LoadSavedUser();
    }

    private void LoadSavedUser()
    {
        string savedUserData = PlayerPrefs.GetString("SavedUser");
        if (!string.IsNullOrEmpty(savedUserData))
        {
            savedUser = JsonUtility.FromJson<User>(savedUserData);
        }
    }

    private void SaveUser(User user)
    {
        string json = JsonUtility.ToJson(user);
        PlayerPrefs.SetString("SavedUser", json);
        PlayerPrefs.Save();
    }

    public void Register()
    {
        string enteredUsername = usernameInput.text;
        string enteredPassword = passwordInput.text;
        string confirmedPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword) || string.IsNullOrEmpty(confirmedPassword))
        {
            registrationMessageText.text = "Please enter username and password.";
            return;
        }

        if (enteredPassword != confirmedPassword)
        {
            registrationMessageText.text = "Passwords do not match.";
            return;
        }

        User newUser = new User
        {
            username = enteredUsername,
            password = enteredPassword
        };

        SaveUser(newUser);
        registrationMessageText.text = "Registration successful!";
    }
}
