import { Outlet } from "react-router";
import PublicNavbar from "./PublicNavbar";
import Footer from "../common/Footer";


function PublicLayout() {
    return (
        <div className="d-flex flex-column min-vh-100">
            <PublicNavbar />

            <main className="flex-grow-1">
                <Outlet />
            </main>

            <Footer />
        </div>
    );
}

export default PublicLayout;