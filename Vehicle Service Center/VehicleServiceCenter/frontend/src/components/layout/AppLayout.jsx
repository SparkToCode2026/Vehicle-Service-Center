import { Outlet } from "react-router";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";

function AppLayout() {
  return (
    <div className="container-fluid min-vh-100 bg-light">
      <div className="row min-vh-100">
        <Sidebar />

        <div className="col p-0 d-flex flex-column">
          <Navbar />

          <main className="flex-grow-1 p-4">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}

export default AppLayout;
