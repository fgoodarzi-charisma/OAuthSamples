import withAuth from "@/hoc/withAuth";
import Link from "next/link";
import { useAuth } from "react-oidc-context";

const Secure = () => {
    const auth = useAuth();

    return (
        <div>
            <h1>This is a Secure page</h1>
            <div>
                <Link href="/">Home</Link>
            </div>
            <div>
                <pre>
                    <h4>Profile:</h4>
                    {JSON.stringify(auth.user?.profile, null, 2)}

                    <h4>Id Token:</h4>
                    <div style={{ minWidth: "250px", maxWidth: "50%", overflowWrap: "break-word", whiteSpace: "initial" }}>
                        {auth.user?.id_token}
                    </div>

                    <h4>Access Token:</h4>
                    <div style={{ minWidth: "250px", maxWidth: "50%", overflowWrap: "break-word", whiteSpace: "initial" }}>
                        {auth.user?.access_token}
                    </div>
                </pre>
            </div>
        </div>
    )
}

export default withAuth(Secure);