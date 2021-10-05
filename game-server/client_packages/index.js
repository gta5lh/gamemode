mp.events.add({
  CheckAuthToken: () => {
    if (mp.storage.data.auth) {
      mp.events.callLocal(
        "UseAuthToken",
        mp.storage.data.auth.email,
        mp.storage.data.auth.token
      );
    }
  },

  SaveAuthToken: (email, token) => {
    mp.storage.data.auth = {
      email: email,
      token: token,
    };
  },
});
