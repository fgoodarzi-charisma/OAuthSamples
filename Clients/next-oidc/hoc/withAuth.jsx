import { useRouter } from "next/router";
import { useAuth } from "react-oidc-context";

const withAuth = (WrappedComponent) => {
    return (props) => {
        const auth = useAuth();
        const router = useRouter();

        if (auth.isLoading) {
            return <></>
        }
    
        if (!auth.isAuthenticated) {
            return (
                <>
                    <button onClick={() => auth.signinRedirect({ state: router.pathname, })}>Login</button>
                    <h1>You need to login</h1>
                </>
            )
        }

        return (
            <>
                <button onClick={auth.removeUser}>Logout</button>
                <WrappedComponent {...props} />
            </>
        )
    }
}

export default withAuth;