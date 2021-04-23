"use strict";
/// <reference types="PFBridgeCore" />
const a = System.IO;
a.File.WriteAllText("aa.json", "", System.Text.Encoding.UTF8);
const x = a.File.ReadAllText;
