const IDENTITY_CONFIG = {
    authority: process.env.NEXT_PUBLIC_AUTH_URL,
    client_id: process.env.NEXT_PUBLIC_IDENTITY_CLIENT_ID,
    redirect_uri: process.env.NEXT_PUBLIC_CALLBACK_URL,
    response_type: "code", 
    scope: "openid profile weather",
    automaticSilentRenew: true,
    silent_redirect_uri: process.env.NEXT_PUBLIC_SILENT_CALLBACK_URL,
}

export default IDENTITY_CONFIG;