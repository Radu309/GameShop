import "../css/users/UsersList.css"

function UsersList() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    // Funcție pentru a obține lista de utilizatori
    async function fetchUsers() {
        try {
            const response = await fetch('/api/AppUser');
            if (response.ok) {
                const data = await response.json();
                setUsers(data);
            } else {
                console.error('Failed to fetch users');
            }
        } catch (error) {
            console.error('Error fetching users:', error);
        } finally {
            setLoading(false);
        }
    }

    // Funcție pentru editarea unui utilizator
    function editUser(userId) {
        window.location.href = `/AppUser/Edit/${userId}`;
    }

    // Funcție pentru ștergerea unui utilizator
    async function deleteUser(userId) {
        const response = await fetch(`/api/AppUser/${userId}`, {
            method: 'DELETE',
        });

        if (response.ok) {
            alert('User deleted successfully!');
            fetchUsers(); // Reîncarcă lista de utilizatori
        } else {
            alert('Failed to delete user');
        }
    }

    // Apelează funcția pentru a obține utilizatorii atunci când componenta este montată
    useEffect(() => {
        fetchUsers();
    }, []);

    return (
        <section id="users-list">
            <div className="users-container">
                <h1>Users List</h1>
                {loading ? (
                    <p>Loading...</p>
                ) : (
                    <>
                        <div className="user-attributes-header">
                            <div className="user-attribute-header"><h3>Full Name</h3></div>
                            <div className="user-attribute-header"><h3>Email</h3></div>
                            <div className="user-attribute-header"><h3>Orders</h3></div>
                            <div className="user-attribute-header"><h3>Actions</h3></div>
                        </div>
                        <ul className="list">
                            {users.map((user) => (
                                <li key={user.id} className="list-item">
                                    <div className="user-info">
                                        <div className="user-attribute">
                                            <h3>{user.fullName}</h3>
                                        </div>
                                        <div className="user-attribute">
                                            <p>{user.email}</p>
                                        </div>
                                        <div className="user-attribute">
                                            <p>{user.orders.length} Orders</p>
                                        </div>
                                        <div className="user-attribute">
                                            <button onClick={() => editUser(user.id)}>Edit</button>
                                            <button onClick={() => deleteUser(user.id)}>Delete</button>
                                        </div>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    </>
                )}
            </div>
        </section>
    );
}


export default UsersList;