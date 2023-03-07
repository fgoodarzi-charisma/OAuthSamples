import "@/styles/main.css"
import IDENTITY_CONFIG from "@/config"
import { AuthProvider } from "react-oidc-context"

export default function App({ Component, pageProps }) {
  return (
    <AuthProvider {...IDENTITY_CONFIG }>
      <Component {...pageProps} />
    </AuthProvider>
  )
}
