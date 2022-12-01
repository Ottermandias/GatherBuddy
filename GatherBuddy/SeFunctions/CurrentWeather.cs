using Dalamud.Game;
using Dalamud.Utility.Signatures;

namespace GatherBuddy.SeFunctions;

public sealed class CurrentWeather : SeAddressBase
{
    // Discovery notes:
    //
    // Weather is a one byte value in memory that corresponds to the Weather table.
    // There are duplicate names in that table, but all of the ARR overworld zones use
    // the first weather value of that type, usually 1-10.  There's ~4 static memory locations.
    // One of these locations is the predicted weather (precalculated based on zone and time)
    // and the other three are the true weather (based on in-game compass / zone effects / etc).
    // You can use Bismark hard mode to differentiate these, as Bismark's weather
    // effects only affect the true weather.  All the reads/writes of these values come
    // in subroutines so there's nothing simpler than something like the below:
    //
    // ffxiv_dx11.exe+6E2759 - C0 EA 04              - shr dl,04 { 4 }
    // ffxiv_dx11.exe+6E275C - 80 E2 01              - and dl,01 { 1 }
    // ffxiv_dx11.exe+6E275F - E8 1C0A2200           - call ffxiv_dx11.exe+903180
    // ffxiv_dx11.exe+6E2764 - 44 0FB7 4D 12         - movzx r9d,word ptr [rbp+12]
    // ffxiv_dx11.exe+6E2769 - 48 8D 0D 90CC9001     - lea rcx,[ffxiv_dx11.exe+1FEF400] { (7FF7A5C2F418) }
    // ffxiv_dx11.exe+6E2770 - 44 0FB7 45 02         - movzx r8d,word ptr [rbp+02]
    // ffxiv_dx11.exe+6E2775 - 0FB6 55 10            - movzx edx,byte ptr [rbp+10]
    // ffxiv_dx11.exe+6E2779 - 66 41 C1 E9 09        - shr r9w,09 { 9 }
    // ffxiv_dx11.exe+6E277E - 41 80 E1 01           - and r9l,01 { 1 }
    // ffxiv_dx11.exe+6E2782 - E8 69CB5400           - call ffxiv_dx11.exe+C2F2F0
    //
    // ffxiv_dx11.exe+C2F2F0 - 48 89 5C 24 08        - mov [rsp+08],rbx
    // ffxiv_dx11.exe+C2F2F5 - 48 89 6C 24 10        - mov [rsp+10],rbp
    // ffxiv_dx11.exe+C2F2FA - 48 89 74 24 18        - mov [rsp+18],rsi
    // ffxiv_dx11.exe+C2F2FF - 57                    - push rdi
    // ffxiv_dx11.exe+C2F300 - 48 83 EC 20           - sub rsp,20 { 32 }
    // ffxiv_dx11.exe+C2F304 - 88 51 50              - mov [rcx+50],dl
    public CurrentWeather(SigScanner sigScanner)
        : base(sigScanner, "44 0FB7 4D 12 48 8D 0D", 0x50)
    {
        SignatureHelper.Initialise(this);
    }

    public unsafe byte Current
        => *(byte*)Address;
}
