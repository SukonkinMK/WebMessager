namespace UserService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<UserEntity> Users { get; set; }
    }
}
