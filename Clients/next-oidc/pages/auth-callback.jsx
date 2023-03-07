import { useRouter } from "next/router";
import { useEffect } from "react";
import { useAuth } from "react-oidc-context";

const AuthCallback = () => {
    const router = useRouter();
    const auth = useAuth();

    useEffect(() => {
        if (auth.isAuthenticated) {
            router.push(auth.user.state);
        }
    })

    return <></>
}

export default AuthCallback;