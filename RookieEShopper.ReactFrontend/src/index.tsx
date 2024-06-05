import React from "react";
import { createRoot } from "react-dom/client";
import { AuthProvider } from "react-oidc-context";

import App from "./App";

const container = document.getElementById("root") as HTMLElement;
const root = createRoot(container);

const oidcConfig = {
  authority: "https://localhost:8899",
  client_id: "spa",
  client_secret: "secret",
  redirect_uri: window.location.origin,
  scope: "openid email profile manage roles",
  post_logout_redirect_uri: window.location.origin,
};

root.render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig}>
      <App />
    </AuthProvider>
  </React.StrictMode>
);
