// Handle events from client
function Trigger(eventName, args) {
  try {
    var handlers = window.EventManager.events[eventName];
    if (handlers) {
      handlers.forEach((handler) =>
        handler(args || args == 0 ? JSON.parse(args) : "")
      );
    }
  } catch (e) {
    console.log(e);
  }
}

// Capt
function InitGangWar(value) {
  Trigger("InitGangWar", value);
}

function StartGangWar(value) {
  Trigger("StartGangWar", value);
}

function UpdateStats(value) {
  Trigger("UpdateStats", value);
}

function HideCapt() {
  Trigger("HideCapt");
}

// NPC
function SetNpcDialogue(value) {
  Trigger("SetNpcDialogue", value);
}

function InitNpcDialogue(value) {
  Trigger("InitNpcDialogue", value);
}

function CloseNpcDialogue() {
  Trigger("CloseNpcDialogue");
}

// Hud
function SetZoneState(enabled, color) {
  let state = {
    enabled: enabled,
    color: color,
  };

  Trigger("SetZoneState", JSON.stringify(state));
}

function HideHelpMenu() {
  Trigger("HelpMenu", JSON.stringify({ type: "hide" }));
}

function ShowHelpMenu() {
  Trigger("HelpMenu", JSON.stringify({ type: "show" }));
}

function HideHud() {
  Trigger("Hud", JSON.stringify({ type: "hide" }));
}

function ShowHud() {
  Trigger("Hud", JSON.stringify({ type: "show" }));
}

function UpdateOnline(online) {
  Trigger("UpdateOnline", online);
}

function UpdateSpeedometer(speed) {
  Trigger("UpdateSpeedometer", speed);
}

function ShowSpeedometer() {
  Trigger("Speedometer", JSON.stringify({ type: "show" }));
}

function HideSpeedometer() {
  Trigger("Speedometer", JSON.stringify({ type: "hide" }));
}

function HideVoice() {
  Trigger("Voice", JSON.stringify({ type: "hide" }));
}

function ShowVoice() {
  Trigger("Voice", JSON.stringify({ type: "show" }));
}

function UpdateMoney(money) {
  Trigger("UpdateMoney", money);
}

function UpdateTime(hours, minutes, day, month) {
  let dateTime = {
    hours: hours,
    minutes: minutes,
    day: day,
    month: month,
  };

  Trigger("UpdateTime", JSON.stringify(dateTime));
}

// Auth
function ResetPasswordSucceed() {
  Trigger("ResetPasswordSucceed");
}

function ResetPasswordFailed(errors) {
  Trigger("ResetPasswordFailed", errors);
}

function RegisterFailed(errors) {
  Trigger("RegisterFailed", errors);
}

function LoginFailed(errors) {
  Trigger("LoginFailed", errors);
}

function HideAuth() {
  Trigger("Auth", JSON.stringify({ type: "hide" }));
}

function ShowAuth() {
  Trigger("Auth", JSON.stringify({ type: "show" }));
}
