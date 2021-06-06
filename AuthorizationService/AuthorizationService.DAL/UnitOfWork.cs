using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL.Repositories;

namespace AuthorizationService.DAL
{
	public class UnitOfWork : IUnitOfWork
	{

		private AuthorizationContext db { get; set; }

		private UserRepository _userRepository { get; set; }
		private TaskRepository _taskRepository { get; set; }
		private MessageRepository _messageRepository { get; set; }

		public UnitOfWork(string connectionString)
		{

			var optionsBuilder = new DbContextOptionsBuilder<AuthorizationContext>();

			var options = optionsBuilder.UseSqlServer(connectionString).Options;

			db = new AuthorizationContext(options);
		}

		public UnitOfWork(DbContextOptions<AuthorizationContext> options)
		{
			db = new AuthorizationContext(options);
		}

		public IRepository<UserDAL> Users
		{
			get
			{
				if (_userRepository == null)
					_userRepository = new UserRepository(db);
				return _userRepository;
			}
		}

		public IRepository<TaskDAL> Tasks
		{
			get
			{
				if (_taskRepository == null)
					_taskRepository = new TaskRepository(db);
				return _taskRepository;
			}
		}

		public IRepository<MessageDAL> Messages
		{
			get
			{
				if (_messageRepository == null)
					_messageRepository = new MessageRepository(db);
				return _messageRepository;
			}
		}

		public void Save() => db.SaveChanges();

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
				this.disposed = true;
			}

		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
