import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import BirthdayList from './BirthdayList';
import AddBirthday from './AddBirthday';
import EditBirthday from './EditBirthday';

function Home() {
  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Сегодняшние и ближайшие дни рождения</h1>
      <div className="mb-4 space-x-4">
        <Link to="/all" className="text-blue-600 hover:underline">Полный список</Link>
        <Link to="/add" className="text-green-600 hover:underline">Добавить</Link>
      </div>
      <h2 className="text-xl font-semibold mb-2">Сегодняшние</h2>
      <BirthdayList endpoint="/api/birthdaypeople/today" />
      <h2 className="text-xl font-semibold mt-6 mb-2">Ближайшие</h2>
      <BirthdayList endpoint="/api/birthdaypeople/upcoming" />
    </div>
  );
}

function AllBirthdays() {
  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Все дни рождения</h1>
      <BirthdayList endpoint="/api/birthdaypeople" />
    </div>
  );
}

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