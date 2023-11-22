using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public static class InputManager
{
    private static Controls _ctrls;
    
    public static float stockpile = 12f;
    public static float force = 12f;



    public static int magazine = 6;
    private static Vector3 _mousePos;

    public static int eswitch = 1;

    private static Camera cam;
    public static Ray GetCameraRay()
    {
        return cam.ScreenPointToRay(_mousePos);
    }

    public static void Init(Player p)
    {
        _ctrls = new();

        cam = Camera.main;
        
        _ctrls.Permenanet.Enable();

        _ctrls.InGame.Shoot.performed += _ =>
        {
            p.Shoot();
        };

        _ctrls.InGame.Reload.performed += _ =>
        {
            if(stockpile >=6) { stockpile -= 6 - magazine; magazine = magazine + (6-magazine);  }
           
        };
        _ctrls.InGame.Refill.performed += _ =>
        {
            stockpile = 12f;
        };
        _ctrls.InGame.WeaponSwitch.performed += _ =>
        {

            force = 100f;
           
        };
        _ctrls.InGame.EnumSwitch.performed += _ =>
        {
            eswitch++;
            if(eswitch >3) { eswitch = 1; }
        };
        _ctrls.Permenanet.MousePos.performed += ctx =>
        {
            _mousePos = ctx.ReadValue<Vector2>();
        };
    }

    public static void EnableInGame()
    {
        _ctrls.InGame.Enable();
    }
}
