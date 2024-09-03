using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagementSystem.Models;

public class TaskManagementContext : DbContext
{
    public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
    : base(options)
    {
    }

    public DbSet<TaskManagementSystem.Models.Task> Tasks { get; set; }
}
