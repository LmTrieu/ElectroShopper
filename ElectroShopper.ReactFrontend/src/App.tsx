import { AuthProvider, Authenticated, Refine } from "@refinedev/core";
import { DevtoolsProvider } from "@refinedev/devtools";
import { RefineKbar, RefineKbarProvider } from "@refinedev/kbar";

import {
  AuthPage,
  ErrorComponent,
  ThemedLayoutV2,
  ThemedSiderV2,
  ThemedTitleV2,
  useNotificationProvider,
} from "@refinedev/antd";
import "@refinedev/antd/dist/reset.css";

import routerBindings, {
  CatchAllNavigate,
  DocumentTitleHandler,
  NavigateToResource,
  UnsavedChangesNotifier,
} from "@refinedev/react-router-v6";
import { dataProvider } from "./providers/data-provider";
import { App as AntdApp } from "antd";
import { BrowserRouter, Outlet, Route, Routes, redirect } from "react-router-dom";
import { AppIcon } from "./components/app-icon";
import { Header } from "./components/header";
import {
  CategoryCreate,
  CategoryEdit,
  CategoryList,
  CategoryShow,
} from "./pages/categories";
import {
  ProductCreate,
  ProductEdit,
  ProductList,
  ProductShow,
} from "./pages/products";
import { Login } from "./pages/login";
import { useAuth } from "react-oidc-context";


function App() {
  const { isLoading, error, isAuthenticated, activeNavigator, user, removeUser, signinRedirect, signoutRedirect, signoutSilent } = useAuth();
  switch (activeNavigator) {
    case "signinSilent":
      return <div>Signing you in...</div>;
    case "signoutRedirect":
      return <div>Signing you out...</div>;
  }
  if (isLoading) {
    return (
      <div>loading...</div>
    )
  }
  if (error) {
    return;
  }

  const CustomAuthProvider: AuthProvider = {
    login: async () => {
      await signinRedirect();
      if (user && user.access_token)
        localStorage.setItem("access_token", user.access_token);
      return {
        success: true,
      };
    },
    logout: async () => {
      await removeUser();
      await signoutRedirect();
      localStorage.removeItem("access_token");
      return {
        success: true,
        redirectTo: "/login",
      };
    },
    check: async () => {
      if (isAuthenticated && user && user.profile && user.profile.role === 'Admin') {
        if (user && user.access_token)
          localStorage.setItem("access_token", user.access_token);
        return {
          authenticated: true,
        };
      }
      if (user && user.profile.role != 'Admin') {
        alert("User not authorized")
        await signoutRedirect();
        await removeUser();        
        localStorage.removeItem("access_token");

      }
      return {
        authenticated: false,      
        redirectTo: "/",
      };
    },
    getPermissions: async () => {
      if (user && user.profile && user.profile.role) {
        return user.profile.role;
      }
      return null;
    },
    getIdentity: async () => {
      if (user) {
        return {
          id: user.profile.sub,
          name:
            user.profile.name || user.profile.preferred_username || "Admin",
        };
      }
      return null;
    },
    onError: async (error) => {
      console.error(error);
      return { error };
    },
  };

  return (
    <BrowserRouter>
      <RefineKbarProvider>
        <AntdApp>
          <DevtoolsProvider>
            <Refine
              dataProvider={dataProvider}
              notificationProvider={useNotificationProvider}
              authProvider={CustomAuthProvider}
              routerProvider={routerBindings}
              resources={[
                {
                  name: "products",
                  list: "/products",
                  create: "/products/create",
                  edit: "/products/edit/:id",
                  show: "/products/show/:id",
                  meta: {
                    canDelete: true,
                  },
                },
                {
                  name: "categories",
                  list: "/categories",
                  create: "/categories/create",
                  edit: "/categories/edit/:id",
                  show: "/categories/show/:id",
                  meta: {
                    canDelete: true,
                  },
                },
              ]}
              options={{
                syncWithLocation: true,
                warnWhenUnsavedChanges: true,
              }}
            >
              <Routes>
                <Route
                  element={
                    <Authenticated
                      loading={<div>loading...</div>}
                      key="authenticated-inner"
                      fallback={<CatchAllNavigate to="/login" />}
                    >
                      <ThemedLayoutV2
                        Header={() => <Header sticky />}
                        Sider={(props) => <ThemedSiderV2 {...props} fixed />}
                        Title={({ collapsed }) => (
                          <ThemedTitleV2
                            collapsed={collapsed}
                            text="RookieECommerce Admin Page"
                            icon={<AppIcon />}
                          />
                        )}
                      >
                        <Outlet />
                      </ThemedLayoutV2>
                    </Authenticated>
                  }
                >
                  <Route
                    index
                    element={<NavigateToResource resource="products" />}
                  />
                  <Route path="/products">
                    <Route index element={<ProductList />} />
                    <Route path="create" element={<ProductCreate />} />
                    <Route path="edit/:id" element={<ProductEdit />} />
                    <Route path="show/:id" element={<ProductShow />} />
                  </Route>
                  <Route path="/categories">
                    <Route index element={<CategoryList />} />
                    <Route path="create" element={<CategoryCreate />} />
                    <Route path="edit/:id" element={<CategoryEdit />} />
                    <Route path="show/:id" element={<CategoryShow />} />
                  </Route>
                  <Route path="*" element={<ErrorComponent />} />
                </Route>

                <Route
                  element={
                    <Authenticated
                      loading={<div>loading...</div>}
                      key="authenticated-outer"
                      fallback={<Outlet />}
                    >
                      <NavigateToResource />
                    </Authenticated>
                  }
                >
                  <Route path="/login" element={<Login />} />
                </Route>
              </Routes>

              <UnsavedChangesNotifier />
              <DocumentTitleHandler />
              <RefineKbar />
            </Refine>
          </DevtoolsProvider>
        </AntdApp>
      </RefineKbarProvider>
    </BrowserRouter>
  );
}

export default App;
