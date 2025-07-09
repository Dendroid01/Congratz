import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

function groupByMonth(people) {
  const groups = {};
  people.forEach(person => {
    const date = new Date(person.dateOfBirth);
    const month = date.toLocaleString('ru-RU', { month: 'long' });
    if (!groups[month]) groups[month] = [];
    groups[month].push(person);
  });

  Object.keys(groups).forEach(month => {
    groups[month].sort((a, b) => {
      const dayA = new Date(a.dateOfBirth).getDate();
      const dayB = new Date(b.dateOfBirth).getDate();
      return dayA - dayB;
    });
  });

  return groups;
}

export default function AllBirthdays() {
  const [people, setPeople] = useState([]);
  const [sortOption, setSortOption] = useState('date');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const fetchData = () => {
    setLoading(true);
    fetch('/api/birthdaypeople')
      .then(res => {
        if (!res.ok) throw new Error('Ошибка загрузки данных');
        return res.json();
      })
      .then(data => {
        setPeople(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleDelete = async (id) => {
    if (!window.confirm('Удалить запись?')) return;
    try {
      const res = await fetch(`/api/birthdaypeople/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Ошибка при удалении');
      fetchData();
    } catch (err) {
      alert(err.message);
    }
  };

  if (loading) return <div className="p-4">Загрузка...</div>;
  if (error) return <div className="p-4 text-red-600">Ошибка: {error}</div>;
  if (people.length === 0) return <div className="p-4">Записей нет</div>;

  let content = null;

  if (sortOption === 'name') {
    const sortedByName = [...people].sort((a, b) =>
      a.fullName.localeCompare(b.fullName)
    );

    content = (
      <ul className="space-y-2">
        {sortedByName.map(person => (
          <li
            key={person.id}
            className="border p-3 rounded flex items-center gap-4"
          >
            {person.photoBase64 ? (
              <img
                src={`data:${person.photoMimeType};base64,${person.photoBase64}`}
                alt={person.fullName}
                className="w-12 h-12 rounded-full object-cover"
              />
            ) : (
              <div className="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center text-gray-600 text-xs">
                Фото
              </div>
            )}
            <div className="flex-1">
              <div className="font-semibold">{person.fullName}</div>
              <div className="text-sm text-gray-600">
                {new Date(person.dateOfBirth).toLocaleDateString()}
              </div>
            </div>
            <div className="flex flex-col gap-1 text-sm">
              <button
                className="text-blue-600 hover:underline"
                onClick={() => navigate(`/edit/${person.id}`)}
              >
                Редактировать
              </button>
              <button
                className="text-red-600 hover:underline"
                onClick={() => handleDelete(person.id)}
              >
                Удалить
              </button>
            </div>
          </li>
        ))}
      </ul>
    );
  } else {
    const groups = groupByMonth(people);

    const monthOrder = [
      'январь', 'февраль', 'март', 'апрель', 'май', 'июнь',
      'июль', 'август', 'сентябрь', 'октябрь', 'ноябрь', 'декабрь'
    ];

    content = monthOrder
      .filter(month => groups[month])
      .map(month => (
        <div key={month} className="mb-6">
          <h2 className="text-xl font-semibold mb-2 capitalize">{month}</h2>
          <ul className="space-y-2">
            {groups[month].map(person => (
              <li
                key={person.id}
                className="border p-3 rounded flex items-center gap-4"
              >
                {person.photoBase64 ? (
                  <img
                    src={`data:${person.photoMimeType};base64,${person.photoBase64}`}
                    alt={person.fullName}
                    className="w-12 h-12 rounded-full object-cover"
                  />
                ) : (
                  <div className="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center text-gray-600 text-xs">
                    Фото
                  </div>
                )}
                <div className="flex-1">
                  <div className="font-semibold">{person.fullName}</div>
                  <div className="text-sm text-gray-600">
                    {new Date(person.dateOfBirth).toLocaleDateString()}
                  </div>
                </div>
                <div className="flex flex-col gap-1 text-sm">
                  <button
                    className="text-blue-600 hover:underline"
                    onClick={() => navigate(`/edit/${person.id}`)}
                  >
                    Редактировать
                  </button>
                  <button
                    className="text-red-600 hover:underline"
                    onClick={() => handleDelete(person.id)}
                  >
                    Удалить
                  </button>
                </div>
              </li>
            ))}
          </ul>
        </div>
      ));
  }

  return (
    <div className="p-4">
      <div className="mb-4">
        <Link to="/" className="text-blue-600 hover:underline">&larr; На главную</Link>
      </div>
      <h1 className="text-2xl font-bold mb-4">Все дни рождения</h1>

      <div className="mb-4">
        <label className="mr-2 font-medium">Сортировать по:</label>
        <select
          value={sortOption}
          onChange={(e) => setSortOption(e.target.value)}
          className="border rounded p-1"
        >
          <option value="date">Дате рождения (по месяцам)</option>
          <option value="name">ФИО (по алфавиту)</option>
        </select>
      </div>

      {content}
    </div>
  );
}