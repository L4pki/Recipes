//import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Authorization from "./pages/Authorization";
import Main from "./pages/Main";
import Recipes from "./pages/Recipes";
import Detail from "./pages/Detail";
import Profile from "./pages/Profile";
import Favorite from "./pages/Favorite";
//import Navbar from "./components/Navbar";
import "./App.css";
import Header from "./components/Header/Header";
import Footer from "./components/Footer/Footer";
import EditRecipe from "./pages/RecipeEdit";
import RecipeCreate from "./pages/RecipeCreate";
import { useEffect } from "react";
import { refreshAccessToken } from "./api/authService";

function App() {
    useEffect(() => {
        const isTokenExpired = (token: string | null) => {
            if (!token) return true;
            const payload = JSON.parse(atob(token.split('.')[1]));
            const exp = payload.exp * 1000;
            return Date.now() >= exp;
        };

        const tokenCheckInterval = setInterval(() => {
            const token = localStorage.getItem('token');
            if (isTokenExpired(token)) {
                refreshAccessToken();
            }
        }, 300 * 1000);
        return () => clearInterval(tokenCheckInterval);
    }, []);

  return (
      <Router>
          <div className="app-container">
              <Header />
              <main className="content">
                  <Routes>
                      <Route path="/authorization" element={<Authorization />} />
                      <Route path="/main" element={<Main />} />
                      <Route path="/recipes" element={<Recipes />} />
                      <Route path="/detail/:id" element={<Detail />} />
                      <Route path="/RecipeEdit/:id" element={<EditRecipe />} />
                      <Route path="/RecipeCreate/" element={<RecipeCreate />} />
                      <Route path="/profile" element={<Profile />} />
                      <Route path="/favorite" element={<Favorite />} />
                      <Route path="/" element={<h1>Добро пожаловать!</h1>} />
                  </Routes>
              </main>
              <Footer />
          </div>
      </Router>
  );
}

export default App;
