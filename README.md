NfcLib
======

 FeliCa(NfcF) and Mifare(Classic/Ultralight) access library for C#(Sony PaSoRi)

===============================
for FeliCa
==============================
    NfcLib nfclib = new NfcLib();
    nfclib.InitializeLibrary(UseCard.Felica);

    //select system code
    //if not detected return null.
    NfcTag card = nfclib.Polling(0xfe00);

    if (card is Felica)
    {
       //reading System code 0xfe00, Service code 0x110b, Block 0,1,2
       //you can read non sequencial block ex.(0,4,6)
       byte[] buffer = new byte[48];
       ((Felica)card).Read(0x110b, new int[]{0,1,2}, buffer, 0);
    }


===============================
for Mifare Classic
==============================
    NfcLib nfclib = new NfcLib();
    nfclib.InitializeLibrary(UseCard.Mifare);
    
    //if not detected return null.
    NfcTag card = nfclib.Polling();
    if (card is Mifare)
    {
       //authentication block 4(sector 1)
       ((Mifare)card).Authentication(true, 4, new byte[]{0xFF,0xFF,0xFF,0xFF,0xFF,0xFF}); 
       ((Mifare)card).Read(4, buffer, 0);
    
       //reading block 4
       byte[] buffer = new byte[16];
       ((Mifare)card).Read(4, buffer, 0);
    }
