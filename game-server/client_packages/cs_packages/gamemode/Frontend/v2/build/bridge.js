// Handle events from client
function trigger(eventName, args) {
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

// NPC
function SetNpcDialogue(value) {
  trigger("SetNpcDialogue", value);
}

function InitNpcDialogue(value) {
  HideHud();
  trigger("InitNpcDialogue", value);
}

function CloseNpcDialogue() {
  trigger("CloseNpcDialogue");
  ShowHud();
}

// Hud
function SetZoneState(enabled, color) {
  let state = {
    enabled: enabled,
    color: color,
  };

  trigger("SetZoneState", JSON.stringify(state));
}

function HideHelpMenu() {
  trigger("HelpMenu", JSON.stringify({ type: "hide" }));
}

function ShowHelpMenu() {
  trigger("HelpMenu", JSON.stringify({ type: "show" }));
}

function HideHud() {
  trigger("Hud", JSON.stringify({ type: "hide" }));
}

function ShowHud() {
  trigger("Hud", JSON.stringify({ type: "show" }));
}

function UpdateOnline(online) {
  trigger("UpdateOnline", online);
}

function UpdateSpeedometer(speed) {
  trigger("UpdateSpeedometer", speed);
}

function ShowSpeedometer() {
  trigger("Speedometer", JSON.stringify({ type: "show" }));
}

function HideSpeedometer() {
  trigger("Speedometer", JSON.stringify({ type: "hide" }));
}

function HideVoice() {
  trigger("Voice", JSON.stringify({ type: "hide" }));
}

function ShowVoice() {
  trigger("Voice", JSON.stringify({ type: "show" }));
}

function UpdateMoney(money) {
  trigger("UpdateMoney", money);
}

function UpdateTime(hours, minutes, day, month) {
  let dateTime = {
    hours: hours,
    minutes: minutes,
    day: day,
    month: month,
  };

  trigger("UpdateTime", JSON.stringify(dateTime));
}

// Auth
function ResetPasswordSucceed() {
  trigger("ResetPasswordSucceed");
}

function ResetPasswordFailed(errors) {
  trigger("ResetPasswordFailed", errors);
}

function RegisterFailed(errors) {
  trigger("RegisterFailed", errors);
}

function LoginFailed(errors) {
  trigger("LoginFailed", errors);
}

function HideAuth() {
  trigger("Auth", JSON.stringify({ type: "hide" }));
}

function ShowAuth() {
  trigger("Auth", JSON.stringify({ type: "show" }));
}
