"use strict";
/// <reference types="PFBridgeCore" />
let t = new System.Threading.Thread(() => {
});
t.SetApartmentState(0 /* STA */);
t.Start();
