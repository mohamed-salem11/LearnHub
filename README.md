# LearnHub

LearnHub is an e-learning platform that empowers instructors to create and share courses while giving students a seamless way to discover, purchase, and learn online.  
Built with modern software engineering practices — including Clean Architecture, CQRS, and MediatR — LearnHub provides a scalable, maintainable foundation for interactive education.

---

##  Features
- **User Management**: Registration, authentication, and role-based access (student, instructor, admin).
- **Instructor Requests**: Users can apply to become instructors; admins review and approve/reject requests.
- **Course Management**: Instructors add, edit, delete courses with categories and cover images.
- **Category Management (Admin)**: Admins can create and manage course categories.
- **Course Review Workflow (Admin)**: Instructors submit courses for review; admins approve or reject before publishing.
- **Course Access**: Students browse and enroll in approved courses.
- **Payments**: Secure course purchases via Stripe.
- **Course Ratings**: Students rate, update, or remove ratings.
- **Instructor Profiles**: Public pages showcasing instructor courses.
- **Search**: Find courses by title, description, category, or instructor name.


---

##  Tech Stack
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Identity Framework (roles & authentication)
- MediatR (CQRS implementation)
- Clean Architecture (Domain, Application, Infrastructure, Web layers)
- Stripe API (payments)
- Bootstrap & Razor Views (responsive UI)

---
 
