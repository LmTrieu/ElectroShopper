// import React, { useContext } from "react";
// import { AuthProvider } from "@refinedev/core";
// import { AuthContext, useAuth } from "react-oidc-context";


// const CustomAuthProvider: AuthProvider = {
//     login: async () => {
//         const oidc = useAuth();
//         oidc.signinRedirect();
//         return {
//             success: true,
//         };
//     },
//     logout: async () => {
//         const oidc = useAuth();
//         await oidc.signoutRedirect();
//         return {
//             success: true,
//             redirectTo: "/login",
//         };
//     },
//     check: async () => {
//         const oidc = useAuth();
//         const isAuthenticated = oidc.isAuthenticated;
//         if (isAuthenticated) {
//             return {
//                 authenticated: true,
//             };
//         }
//         return {
//             authenticated: false,
//             redirectTo: "/login",
//         };
//     },
//     getPermissions: async () => null,
//     getIdentity: async () => {
//         const oidc = useAuth();
//         const user = oidc.user;
//         if (user) {
//             return {
//                 id: user.profile.sub,
//                 name: user.profile.name || user.profile.preferred_username || "Unknown",
//             };
//         }
//         return null;
//     },
//     onError: async (error) => {
//         console.error(error);
//         return { error };
//     },
// };

// export default CustomAuthProvider;
