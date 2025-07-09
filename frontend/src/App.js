import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import AllBirthdays from './pages/AllBirthdays';
import AddBirthday from './pages/AddBirthday';
import EditBirthday from './pages/EditBirthday';

export default function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/all" element={<AllBirthdays />} />
        <Route path="/add" element={<AddBirthday />} />
        <Route path="/edit/:id" element={<EditBirthday />} />
      </Routes>
    </Router>
  );
}